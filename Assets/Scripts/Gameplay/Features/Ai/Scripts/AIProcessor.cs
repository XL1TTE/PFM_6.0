using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Project.AI
{
    [Serializable]
    public class AIConfig
    {

        public float m_PointPerDistanceFromEnemy;
        public float m_PointsPerDamage;
        public float m_PointsPerHeal;


        [HideInInspector][Range(0f, 1f)] public const float m_RotationWeight = 0f;

        [HideInInspector][Range(0f, 1f)] public const float m_DoNothingWeight = 0f;


        [Header("Strategy")]
        [Range(0.1f, 1f)] public float m_DiscountFactor = 1f;

        [Header("Action Limits")]
        public int m_MaxActionsPerTurn = 3;
        public int m_PlanningDepth = 2;

    }

    public struct AIAction
    {
        public AbilityData m_AbilityData;
        public Entity m_Target;
        public float m_Weight;
        public ActionType m_ActionType;
    }

    public interface IAIProcessor
    {
        UniTask Process(Entity a_agent, World a_world);
    }

    public sealed class TCAI : IAIProcessor
    {
        public async UniTask Process(Entity a_agent, World a_world)
        {
            var context = new AIExecutionContext(a_agent, a_world);
            await new EndTurn().DoJob(context);

        }
    }

    public sealed class AIProcessor : IAIProcessor
    {
        [SerializeField] private AIConfig m_Config;

        public async UniTask Process(Entity a_agent, World a_world)
        {

            using var cts = new CancellationTokenSource();
            var stash_aiCancell = a_world.GetStash<AgentAICancellationToken>();

            stash_aiCancell.Set(a_agent, new AgentAICancellationToken { m_TokenSource = cts });

            var context = new AIExecutionContext(a_agent, a_world);

            try
            {
                var t_bestSequence = await FindBestActionSequence(a_agent, a_world, m_Config.m_MaxActionsPerTurn * m_Config.m_PlanningDepth);
                var t_counter = m_Config.m_MaxActionsPerTurn;

                foreach (var t_action in t_bestSequence)
                {
                    await UniTask.DelayFrame(1, cancellationToken: cts.Token);

                    --t_counter;
                    await ExecuteAction(t_action, context);
                    if (t_counter <= 0)
                    {
                        break;
                    }
                }
                await new EndTurn().DoJob(context);
                if (stash_aiCancell.IsDisposed == false)
                {
                    stash_aiCancell.Remove(a_agent);
                }
            }
            catch (OperationCanceledException)
            {
                if (stash_aiCancell.IsDisposed == false)
                {
                    stash_aiCancell.Remove(a_agent);
                }
            }
        }

        private async UniTask<List<AIAction>> FindBestActionSequence(Entity a_agent, World a_world, int a_depth)
        {
            var t_context = new AIExecutionContext(a_agent, a_world);
            var t_allSequences = await GenerateAllActionSequences(t_context, a_depth);

            return t_allSequences.OrderByDescending(seq => CalculateSequenceWeight(seq, t_context))
                               .FirstOrDefault() ?? new List<AIAction>();
        }

        private async UniTask<List<List<AIAction>>> GenerateAllActionSequences(AIExecutionContext a_context, int a_depth)
        {
            var t_sequences = new List<List<AIAction>>();
            await GenerateSequencesRecursive(
                a_context,
                new List<AIAction>(),
                t_sequences,
                a_depth,
                new AgentState(a_context.m_Agent, a_context.m_World));
            return t_sequences;
        }

        private async UniTask GenerateSequencesRecursive(AIExecutionContext a_context, List<AIAction> a_currentSequence,
                                              List<List<AIAction>> a_allSequences, int a_depthRemaining, AgentState a_currentState)
        {

            if (a_depthRemaining <= 0)
            {
                a_allSequences.Add(new List<AIAction>(a_currentSequence));
                return;
            }

            var t_availableActions = await GetAvailableActions(a_context, a_currentState);

            if (!t_availableActions.Any())
            {
                a_allSequences.Add(new List<AIAction>(a_currentSequence));
                return;
            }

            bool t_hasValidActions = false;

            foreach (var t_action in t_availableActions)
            {
                if (!CanAddActionToSequence(a_currentSequence, t_action))
                    continue;

                t_hasValidActions = true;
                a_currentSequence.Add(t_action);
                var t_newState = SimulateAction(a_currentState, t_action, a_context);
                await GenerateSequencesRecursive(a_context, a_currentSequence, a_allSequences, a_depthRemaining - 1, t_newState);
                a_currentSequence.RemoveAt(a_currentSequence.Count - 1);
            }


            if (!t_hasValidActions)
            {
                a_allSequences.Add(new List<AIAction>(a_currentSequence));
            }
        }

        private bool CanAddActionToSequence(List<AIAction> a_sequence, AIAction a_newAction)
        {
            var t_currentTurnActions = GetCurrentTurnActions(a_sequence);

            var t_movementActionsCount = t_currentTurnActions.Count(a => a.m_ActionType == ActionType.Movement);
            var t_interactionActionsCount = t_currentTurnActions.Count(a => a.m_ActionType == ActionType.Interaction);
            var t_rotationActionsCount = t_currentTurnActions.Count(a => a.m_ActionType == ActionType.Rotation);
            var t_doNothingCount = t_currentTurnActions.Count(a => a.m_ActionType == ActionType.Nothing);

            if (a_newAction.m_ActionType == ActionType.Movement && t_movementActionsCount + t_doNothingCount + t_rotationActionsCount >= 1)
                return false;
            if (a_newAction.m_ActionType == ActionType.Interaction && t_interactionActionsCount + t_doNothingCount >= 1)
                return false;
            if (a_newAction.m_ActionType == ActionType.Rotation && t_movementActionsCount + t_rotationActionsCount + t_doNothingCount >= 1)
                return false;

            return true;
        }

        private List<AIAction> GetCurrentTurnActions(List<AIAction> a_sequence)
        {
            int t_startIndex = Mathf.Max(0, a_sequence.Count - (a_sequence.Count % m_Config.m_MaxActionsPerTurn));
            return a_sequence.Skip(t_startIndex).ToList();
        }

        private async UniTask<List<AIAction>> GetAvailableActions(AIExecutionContext a_context, AgentState a_currentState)
        {
            var t_actions = new List<AIAction>();
            var t_abilities = a_context.m_World.GetStash<AbilitiesComponent>().Get(a_context.m_Agent);

            foreach (var t_ability in t_abilities.GetAllAbilities())
            {
                await AddAbilityActions(a_context, t_ability, t_actions, a_currentState);
            }

            // t_actions.Add(new AIAction
            // {
            //     m_ActionType = ActionType.Nothing,
            //     m_Weight = m_Config.m_DoNothingWeight,
            //     m_AbilityData = L.DO_NOTHING_ABILITY
            // });

            return t_actions;
        }


        private async UniTask AddAbilityActions(AIExecutionContext a_context, AbilityData a_abilityData, List<AIAction> a_actions, AgentState a_currentState)
        {

            if (a_abilityData.m_AbilityType == AbilityType.MOVEMENT)
            {
                AddMovementAbilityActions(a_context, a_abilityData, a_actions, a_currentState);
            }
            else if (a_abilityData.m_AbilityType == AbilityType.INTERACTION)
            {
                await AddInteractionAbilityActions(a_context, a_abilityData, a_actions, a_currentState);
            }
            else if (a_abilityData.m_AbilityType == AbilityType.ROTATE)
            {
                AddRotateAbilityActions(a_context, a_abilityData, a_actions, a_currentState);
            }
        }
        private void AddRotateAbilityActions(AIExecutionContext a_context, AbilityData a_abilityData, List<AIAction> a_actions, AgentState a_currentState)
        {
            var t_rotateAction = new AIAction
            {
                m_AbilityData = a_abilityData,
                m_Weight = AIConfig.m_RotationWeight,
                m_ActionType = ActionType.Rotation
            };
            a_actions.Add(t_rotateAction);
        }

        private void AddMovementAbilityActions(AIExecutionContext a_context, AbilityData a_abilityData, List<AIAction> a_actions, AgentState a_currentState)
        {
            var t_agentPos = a_currentState.m_Position;

            var t_moveShifts = a_currentState.m_IsRotated ?
                a_abilityData.m_Shifts.Select(x => x *= new Vector2Int(-1, 1)) : a_abilityData.m_Shifts;

            var t_moveOptions = GU.GetCellsFromShifts(t_agentPos, t_moveShifts, a_context.m_World)
                .Where(opt => !F.IsOccupiedCell(opt, a_context.m_World)).ToList();

            foreach (var t_cell in t_moveOptions)
            {
                var t_weight = CalculateMovementWeight(GU.GetCellGridPosition(t_cell, a_context.m_World), a_context, a_currentState);
                a_actions.Add(new AIAction
                {
                    m_AbilityData = a_abilityData,
                    m_Target = t_cell,
                    m_Weight = t_weight,
                    m_ActionType = ActionType.Movement
                });
            }
        }

        private async UniTask AddInteractionAbilityActions(AIExecutionContext a_context, AbilityData a_abilityData, List<AIAction> a_actions, AgentState a_currentState)
        {
            var t_targets = GetAbilityTargets(a_context, a_abilityData, a_currentState);

            foreach (var t_target in t_targets)
            {
                var t_weight = await CalculateAbilityWeight(a_abilityData, t_target, a_context, a_currentState);
                a_actions.Add(new AIAction
                {
                    m_AbilityData = a_abilityData,
                    m_Target = t_target,
                    m_Weight = t_weight,
                    m_ActionType = ActionType.Interaction
                });
            }
        }

        private float CalculateMovementWeight(Vector2Int a_targetCell, AIExecutionContext a_context, AgentState a_currentState)
        {
            var t_enemies = GetAllEnemiesOnField(a_context);

            var t_distance = t_enemies.Min(enemy =>
                Vector2Int.Distance(a_targetCell, GU.GetEntityPositionOnCell(enemy, a_context.m_World)));

            return Math.Abs(t_distance) * m_Config.m_PointPerDistanceFromEnemy;
        }

        private async UniTask<float> CalculateAbilityWeight(AbilityData a_abilityData, Entity a_target, AIExecutionContext a_context, AgentState a_currentState)
        {
            float t_weight = 0f;

            foreach (var t_tag in a_abilityData.m_Tags)
            {
                switch (t_tag)
                {
                    case AbilityTags.DAMAGE:
                        var t_expectedDamage = await CalculateExpectedDamage(a_abilityData, a_target, a_context);

                        t_weight += t_expectedDamage * m_Config.m_PointsPerDamage;
                        break;
                    case AbilityTags.HEAL:
                        var t_expectedHeal = await CalculateExpectedHeal(a_abilityData, a_target, a_context);

                        t_weight += t_expectedHeal * m_Config.m_PointsPerHeal;
                        break;
                }
            }
            return t_weight;
        }

        private async UniTask<float> CalculateExpectedDamage(AbilityData a_abilityData, Entity a_target, AIExecutionContext a_context)
        {
            int t_damageTotal = 0;
            foreach (var dmg_effect in a_abilityData.m_Value.GetEffects<DealDamage>())
            {
                await Interactor.CallAll<CalculateDamageInteraction>(async handler =>
                {
                    t_damageTotal = await handler.Execute(
                        a_context.m_Agent,
                        a_target, a_context.m_World,
                        dmg_effect.m_DamageType,
                        dmg_effect.m_BaseDamage);
                });
            }

            return t_damageTotal;
        }

        private UniTask<float> CalculateExpectedHeal(AbilityData a_abilityData, Entity a_target, AIExecutionContext a_context)
        {
            return UniTask.FromResult(0.0f);
        }

        private List<Entity> GetAllEnemiesOnField(AIExecutionContext a_context)
        {
            if (F.IsEnemy(a_context.m_Agent, a_context.m_World))
            {
                return GU.GetAllMonstersOnField(a_context.m_World).ToList();
            }
            else if (F.IsMonster(a_context.m_Agent, a_context.m_World))
            {
                return GU.GetAllEnemiesOnField(a_context.m_World).ToList();
            }
            return new();
        }

        private List<Entity> GetAbilityTargets(AIExecutionContext a_context, AbilityData a_abilityData, AgentState a_currentState)
        {
            var t_targets = new List<Entity>();
            var t_agentPos = a_currentState.m_Position;

            var t_Shifts = a_currentState.m_IsRotated ?
                a_abilityData.m_Shifts.Select(x => x *= new Vector2Int(-1, 1)) : a_abilityData.m_Shifts;

            var t_targetCells = GU.GetCellsFromShifts(t_agentPos, t_Shifts, a_context.m_World);

            foreach (var t_cell in t_targetCells)
            {
                if (IsValidCell(a_abilityData, t_cell, a_context))
                {
                    var t_target = CastCellToTarget(a_abilityData.m_TargetType, t_cell, a_context);
                    t_targets.Add(t_target);
                }
            }

            return t_targets;
        }

        private Entity CastCellToTarget(TargetSelectionTypes a_targetType, Entity a_cell, AIExecutionContext a_context)
        {
            var t_world = a_context.m_World;
            switch (a_targetType)
            {
                case TargetSelectionTypes.CELL_WITH_ENEMY:
                    return GU.GetCellOccupier(a_cell, t_world);
                case TargetSelectionTypes.CELL_WITH_ALLY:
                    return GU.GetCellOccupier(a_cell, t_world);
                case TargetSelectionTypes.CELL_EMPTY:
                    return a_cell;
            }
            return a_cell;
        }
        private bool IsValidCell(AbilityData a_abilityData, Entity a_cell, AIExecutionContext a_context)
        {
            var t_world = a_context.m_World;
            switch (a_abilityData.m_TargetType)
            {
                case TargetSelectionTypes.CELL_WITH_ENEMY:
                    if (F.IsOccupiedCell(a_cell, t_world))
                    {
                        if (F.IsMonster(a_context.m_Agent, t_world))
                        {
                            return F.IsEnemy(GU.GetCellOccupier(a_cell, t_world), t_world);
                        }
                        else
                        {
                            return F.IsMonster(GU.GetCellOccupier(a_cell, t_world), t_world);
                        }
                    }
                    break;
                case TargetSelectionTypes.CELL_WITH_ALLY:
                    if (F.IsOccupiedCell(a_cell, t_world))
                    {
                        if (F.IsMonster(a_context.m_Agent, t_world))
                        {
                            return F.IsMonster(GU.GetCellOccupier(a_cell, t_world), t_world);
                        }
                        else
                        {
                            return F.IsEnemy(GU.GetCellOccupier(a_cell, t_world), t_world);
                        }
                    }
                    break;
                case TargetSelectionTypes.CELL_EMPTY:
                    return !F.IsOccupiedCell(a_cell, t_world);
            }
            return false;
        }

        private float CalculateSequenceWeight(List<AIAction> a_sequence, AIExecutionContext a_context)
        {
            float t_totalWeight = 0f;
            var t_currentState = new AgentState(a_context.m_Agent, a_context.m_World);

            for (int i = 0; i < a_sequence.Count; i++)
            {
                var t_action = a_sequence[i];


                if (t_action.m_ActionType == ActionType.Rotation)
                {
                    if (i + 1 < a_sequence.Count)
                    {
                        if (IsRotationUseful(t_action, a_sequence[i + 1]) == false)
                        {
                            t_action.m_Weight -= 1000f;
                        }
                    }
                    else
                    {
                        t_action.m_Weight -= 1000f;
                    }
                }


                t_totalWeight += t_action.m_Weight * Mathf.Pow(m_Config.m_DiscountFactor, i);
                t_currentState = SimulateAction(t_currentState, t_action, a_context);
            }

            return t_totalWeight;
        }


        private bool IsRotationUseful(AIAction a_rotation, AIAction t_next)
        {
            if (t_next.m_ActionType == ActionType.Nothing
            || t_next.m_ActionType == ActionType.Rotation)
            {
                return false;
            }

            return true;
        }

        private AgentState SimulateAction(AgentState a_currentState, AIAction a_action, AIExecutionContext a_context)
        {
            var t_newState = a_currentState.Clone();

            if (a_action.m_ActionType == ActionType.Movement && a_action.m_Target.IsExist())
            {
                t_newState.m_Position = GU.GetCellGridPosition(a_action.m_Target, a_context.m_World);
            }
            if (a_action.m_AbilityData.m_AbilityType == AbilityType.ROTATE)
            {
                t_newState.m_IsRotated = !t_newState.m_IsRotated;
            }

            return t_newState;
        }

        private async UniTask ExecuteAction(AIAction a_action, AIExecutionContext a_context)
        {

            await a_action.m_AbilityData.m_Value.Execute(a_context.m_Agent, a_action.m_Target, a_context.m_World);
        }
    }

    public struct AgentState
    {
        public Vector2Int m_Position;
        public bool m_IsRotated;

        public AgentState(Entity a_agent, World a_world)
        {
            m_Position = GU.GetEntityPositionOnCell(a_agent, a_world);

            if (a_world.TryGetComponent<LookDirection>(a_agent, out var component))
            {
                m_IsRotated = component.m_Value == Directions.RIGHT ? false : true;
            }
            else
            {
                m_IsRotated = false;
            }
        }

        public AgentState Clone()
        {
            return new AgentState
            {
                m_Position = this.m_Position,
                m_IsRotated = this.m_IsRotated
            };
        }
    }

    public enum ActionType
    {
        Nothing,
        Movement,
        Interaction,
        Rotation
    }

}
