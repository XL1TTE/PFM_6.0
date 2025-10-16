using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain.AbilityGraph;
using Domain.Commands;
using Domain.Extentions;
using Domain.GameEffects;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Gameplay.AbilityGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityGraphExecutionSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_activeAbilities;
        private Request<DealDamageRequest> req_dealDamage;
        private Event<DamageDealtEvent> evt_dmgDealt;
        private Request<ApplyEffectRequest> req_applyEffect;
        private Stash<AbilityExecutionGraph> stash_abilityGraph;
        private Stash<AbilityExecutionState> stash_abilityState;
        private Stash<AbilityIsExecutingTag> stash_abilityExecutingTag;
        private Stash<AbilityExecutionCompletedTag> stash_abilityExecutionCompletedTag;
        private Stash<AbilityCasterComponent> stash_abilityCaster;
        private Stash<AbilityTargetsComponent> stash_abilityTarget;
        private Event<AbilityActivated> evt_AbilityActivated;
        private Event<AbilityExecutionStarted> evt_AbilityExecutionStarted;
        private Event<AbilityExecutionEnded> evt_AbilityExecutionEnded;

        public void OnAwake()
        {
            f_activeAbilities = World.Filter
                .With<AbilityComponent>()
                .With<AbilityExecutionGraph>()
                .With<AbilityExecutionState>()
                .With<AbilityIsExecutingTag>()
                .Build();

            req_dealDamage = World.GetRequest<DealDamageRequest>();
            evt_dmgDealt = World.GetEvent<DamageDealtEvent>();

            req_applyEffect = World.GetRequest<ApplyEffectRequest>();

            stash_abilityGraph = World.GetStash<AbilityExecutionGraph>();
            stash_abilityState = World.GetStash<AbilityExecutionState>();
            stash_abilityExecutingTag = World.GetStash<AbilityIsExecutingTag>();
            stash_abilityExecutionCompletedTag = World.GetStash<AbilityExecutionCompletedTag>();
            stash_abilityCaster = World.GetStash<AbilityCasterComponent>();
            stash_abilityTarget = World.GetStash<AbilityTargetsComponent>();


            evt_AbilityActivated = World.GetEvent<AbilityActivated>();
            evt_AbilityExecutionStarted = World.GetEvent<AbilityExecutionStarted>();
            evt_AbilityExecutionEnded = World.GetEvent<AbilityExecutionEnded>();
        }

        public void OnUpdate(float deltaTime)
        {
            // Notify that ability execution started.
            foreach (var evt in evt_AbilityActivated.publishedChanges)
            {
                NotifyAbilityExecutionStarted(evt.m_Ability);
            }

            foreach (var abilityEntity in f_activeAbilities)
            {
                ProcessAbilityExecution(abilityEntity, deltaTime);
            }
        }

        public void Dispose() { }

        private void ProcessAbilityExecution(Entity abilityEntity, float deltaTime)
        {
            ref var graph = ref stash_abilityGraph.Get(abilityEntity);
            ref var state = ref stash_abilityState.Get(abilityEntity);

            var currentNodeId = state.CurrentNodeId;

            var currentNode = graph.Nodes.Find(n => n.NodeId == currentNodeId);
            if (currentNode.NodeId == -1)
            {
                stash_abilityExecutingTag.Remove(abilityEntity);
                return;
            }

            switch (currentNode.Type)
            {
                case NodeType.Immediate:
                    ExecuteNodeActions(abilityEntity, currentNode);
                    if (currentNode.Transitions.Count > 0)
                    {
                        TransitionToNextNode(abilityEntity, ref state, currentNode.Transitions[0].TargetNodeId);
                        EvaluateExecutionComplete(abilityEntity, ref state, currentNode);
                    }
                    else
                    {
                        CompleteExecution(abilityEntity, ref state);
                    }
                    break;
                case NodeType.WaitForAnimationFrame:
                    EvaluateAnimationFrameConditions(abilityEntity, ref state, currentNode);
                    EvaluateExecutionComplete(abilityEntity, ref state, currentNode);
                    break;
                case NodeType.WaitForDamage:
                    EvaluateDamageDealtConditions(abilityEntity, ref state, currentNode);
                    EvaluateExecutionComplete(abilityEntity, ref state, currentNode);
                    break;
                case NodeType.WaitForEffect:
                    EvaluateEffectAppliedConditions(abilityEntity, ref state, currentNode);
                    EvaluateExecutionComplete(abilityEntity, ref state, currentNode);
                    break;
                case NodeType.End:
                    CompleteExecution(abilityEntity, ref state);
                    break;
            }
        }

        private void ExecuteNodeActions(Entity abilityEntity, ExecutionNode node)
        {
            ref var state = ref stash_abilityState.Get(abilityEntity);
            var caster = stash_abilityCaster.Get(abilityEntity).caster;
            var targets = stash_abilityTarget.Get(abilityEntity).targets;


            foreach (var action in node.Actions)
            {

                switch (action.Type)
                {
                    case ActionType.PlayAnimation:
                        break;
                    case ActionType.DealDamage:
                        TriggerDealDamageAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.ApplyEffect:
                        TriggerApplyEffectAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.PlaySfx:
                        break;
                    case ActionType.SpawnVfx:
                        break;
                }
            }
        }

        private void TriggerApplyEffectAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            var target_index = action.TargetIndex;
            if (action.OnSelf)
            {
                req_applyEffect.Publish(new ApplyEffectRequest
                {
                    Target = caster,
                    Source = caster,
                    AbilitySource = abilityEntity,
                    EffectId = action.EffectID,
                    DurationInTurns = action.EffectDurationInTurns
                });
            }
            else
            {
                if (target_index == -1)
                {
                    foreach (var target in targets)
                    {
                        req_applyEffect.Publish(new ApplyEffectRequest
                        {
                            Target = target,
                            Source = caster,
                            AbilitySource = abilityEntity,
                            EffectId = action.EffectID,
                            DurationInTurns = action.EffectDurationInTurns
                        });
                    }
                }
                else if (target_index < targets.Count() && target_index >= 0)
                {
                    req_applyEffect.Publish(new ApplyEffectRequest
                    {
                        Target = targets[target_index],
                        Source = caster,
                        AbilitySource = abilityEntity,
                        EffectId = action.EffectID,
                        DurationInTurns = action.EffectDurationInTurns
                    });
                }
            }

        }

        private void TriggerDealDamageAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            int target_index = action.TargetIndex;
            if (target_index == -1)
            {
                foreach (var target in targets)
                {
                    req_dealDamage.Publish(new DealDamageRequest
                    {
                        Source = caster,
                        SourceAbility = abilityEntity,
                        Target = target,
                        MinBaseDamage = action.MinDamageValue,
                        MaxBaseDamage = action.MaxDamageValue,
                        DamageType = action.DamageType
                    });
                }
            }
            else if (target_index < targets.Count() && target_index >= 0)
            {
                req_dealDamage.Publish(new DealDamageRequest
                {
                    Source = caster,
                    SourceAbility = abilityEntity,
                    Target = targets[target_index],
                    MinBaseDamage = action.MinDamageValue,
                    MaxBaseDamage = action.MaxDamageValue,
                    DamageType = action.DamageType
                });
            }
        }

        private void EvaluateExecutionComplete(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            if (currentNode.Transitions.Count < 1)
            {
                CompleteExecution(abilityEntity, ref state);
            }
        }

        private void CompleteExecution(Entity abilityEntity, ref AbilityExecutionState state)
        {
            stash_abilityExecutingTag.Remove(abilityEntity);
            stash_abilityExecutionCompletedTag.Add(abilityEntity);

            NotifyAbilityExecutionEnded(abilityEntity);
        }

        private void NotifyAbilityExecutionStarted(Entity ability)
        {
            // Notify that ability execution ended.
            var caster = stash_abilityCaster.Get(ability).caster;
            evt_AbilityExecutionStarted.NextFrame(new AbilityExecutionStarted
            {
                m_Caster = caster,
                m_Ability = ability
            });
        }
        private void NotifyAbilityExecutionEnded(Entity ability)
        {
            // Notify that ability execution ended.
            var caster = stash_abilityCaster.Get(ability).caster;
            evt_AbilityExecutionEnded.NextFrame(new AbilityExecutionEnded
            {
                m_Caster = caster,
                m_Ability = ability
            });
        }

        private void TransitionToNextNode(Entity abilityEntity, ref AbilityExecutionState state, int targetNodeId)
        {
            state.CurrentNodeId = targetNodeId;

            state.AnimationFrameReached = false;
            state.DamageDealt = false;
            state.EffectApplied = false;
            state.CustomConditionMet = false;
            state.AnimationNotExist = false;
            state.ExecutionTimer = 0f;
        }

        private void EvaluateAnimationFrameConditions(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            if (state.AnimationNotExist)
            {
                TransitionToNextNode(abilityEntity, ref state, currentNode.Transitions[0].TargetNodeId);
                return;
            }

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.Type == ConditionType.AnimationFrame)
                {

                    bool isConditionMet = false;

                    isConditionMet = IsCompared(transition.Condition.IntValue,
                        state.CurrentAnimationFrame, transition.Condition.Operator);

                    if (isConditionMet)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }
            }
        }

        private void EvaluateDamageDealtConditions(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            if (state.DamageDealt == false) { return; }

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.Type == ConditionType.DamageDealt)
                {

                    bool isConditionMet = false;

                    isConditionMet = IsCompared(state.LastDamageAmount, transition.Condition.FloatValue
                        , transition.Condition.Operator);

                    if (isConditionMet)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }
            }
        }

        private void EvaluateEffectAppliedConditions(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            if (state.EffectApplied == false) { return; }

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.Type == ConditionType.EffectApplied)
                {
                    if (transition.Condition.StringValue == state.LastAppliedEffectId)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }
            }
        }




        private bool IsCompared(float a, float b, ComparisonOperator Operator)
        {
            switch (Operator)
            {
                case ComparisonOperator.LesserThen:
                    if (a < b) { return true; }
                    else { return false; }
                case ComparisonOperator.GreaterThen:
                    if (a > b) { return true; }
                    else { return false; }
                case ComparisonOperator.GreaterOrEqual:
                    if (a >= b) { return true; }
                    else { return false; }
                case ComparisonOperator.LesserOrEqual:
                    if (a <= b) { return true; }
                    else { return false; }
                case ComparisonOperator.Equals:
                    if (a == b) { return true; }
                    else { return false; }
            }
            return false;
        }
    }
}


