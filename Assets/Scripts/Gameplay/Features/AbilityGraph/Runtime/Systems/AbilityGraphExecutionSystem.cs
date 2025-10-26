using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain.AbilityGraph;
using Domain.Services;
using Domain.Extentions;
using Domain.GameEffects;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;
using Domain.Stats.Requests;

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
        private Request<AnimatingRequest> req_Animating;
        private Request<AnimateWithTweenRequest> req_AnimateWithTween;
        private Request<TurnAroundRequest> req_TurnAround;
        private Request<ConsumeMovementRequest> req_ConsumeMovement;
        private Request<ConsumeInteractionRequest> req_ConsumeInteraction;
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

            req_Animating = World.GetRequest<AnimatingRequest>();
            req_AnimateWithTween = World.GetRequest<AnimateWithTweenRequest>();

            req_TurnAround = World.GetRequest<TurnAroundRequest>();

            req_ConsumeMovement = World.GetRequest<ConsumeMovementRequest>();
            req_ConsumeInteraction = World.GetRequest<ConsumeInteractionRequest>();

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

            var currentNodeId = state.m_CurrentNodeId;

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
                case NodeType.WaitForAnimation:
                    EvaluateAnimationState(abilityEntity, ref state, currentNode);
                    EvaluateExecutionComplete(abilityEntity, ref state, currentNode);
                    break;
                case NodeType.WaitForTweenInteractionFrame:
                    EvaluateTweenInteractionFrame(abilityEntity, ref state, currentNode);
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
            var targets = stash_abilityTarget.Get(abilityEntity).m_Targets;


            foreach (var action in node.Actions)
            {
                switch (action.m_Type)
                {
                    case ActionType.PlayAnimation:
                        TriggerAnimationPlay(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.PlayTween:
                        TriggerTweenAnimation(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.DealDamage:
                        TriggerDealDamageAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.ApplyEffect:
                        TriggerApplyEffectAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.TurnAround:
                        TriggerTurnAroundAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.ConsumeInteractionAction:
                        ConsumeInteractionAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.ConsumeMovementAction:
                        ConsumeMovementAction(caster, targets, action, abilityEntity);
                        break;
                    case ActionType.PlaySfx:
                        break;
                    case ActionType.SpawnVfx:
                        break;
                }
            }
        }

        private void ConsumeMovementAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            req_ConsumeMovement.Publish(new ConsumeMovementRequest
            {
                m_Subject = caster
            });
        }

        private void ConsumeInteractionAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            req_ConsumeInteraction.Publish(new ConsumeInteractionRequest
            {
                m_Subject = caster
            });
        }

        private void TriggerTurnAroundAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            req_TurnAround.Publish(new TurnAroundRequest
            {
                m_Subject = caster
            });
        }

        private void TriggerAnimationPlay(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            req_TurnAround.Publish(new TurnAroundRequest
            {
                m_Subject = caster,
            });
        }
        private void TriggerTweenAnimation(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            req_AnimateWithTween.Publish(new AnimateWithTweenRequest
            {
                m_Target = targets[action.m_TargetIndex],
                m_TweenAnimationCode = action.m_TweenAnimationCode,
                m_Subject = caster
            });
        }

        private void TriggerApplyEffectAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            var target_index = action.m_TargetIndex;
            if (action.m_OnSelf)
            {
                req_applyEffect.Publish(new ApplyEffectRequest
                {
                    Target = caster,
                    Source = caster,
                    AbilitySource = abilityEntity,
                    EffectId = action.m_EffectID,
                    DurationInTurns = action.m_EffectDurationInTurns
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
                            EffectId = action.m_EffectID,
                            DurationInTurns = action.m_EffectDurationInTurns
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
                        EffectId = action.m_EffectID,
                        DurationInTurns = action.m_EffectDurationInTurns
                    });
                }
            }

        }

        private void TriggerDealDamageAction(Entity caster, Entity[] targets, ActionData action, Entity abilityEntity)
        {
            int target_index = action.m_TargetIndex;
            if (target_index == -1)
            {
                foreach (var target in targets)
                {
                    req_dealDamage.Publish(new DealDamageRequest
                    {
                        Source = caster,
                        SourceAbility = abilityEntity,
                        Target = target,
                        MinBaseDamage = action.m_MinDamageValue,
                        MaxBaseDamage = action.m_MaxDamageValue,
                        DamageType = action.m_DamageType
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
                    MinBaseDamage = action.m_MinDamageValue,
                    MaxBaseDamage = action.m_MaxDamageValue,
                    DamageType = action.m_DamageType
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
            state.m_CurrentNodeId = targetNodeId;

            state.m_DamageDealt = false;
            state.m_EffectApplied = false;
            state.m_CustomConditionMet = false;
            state.m_AnimatingStatus = AnimatingStatus.NONE;
            state.m_IsTweenInteractionFrame = false;
            state.m_ExecutionTimer = 0f;
        }



        private void EvaluateTweenInteractionFrame(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.m_Type == ConditionType.TweenInteractionFrame)
                {
                    if (state.m_IsTweenInteractionFrame)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }

            }
        }

        private void EvaluateAnimationState(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.m_Type == ConditionType.AnimationSuccsesed)
                {
                    if (state.m_AnimatingStatus == AnimatingStatus.SUCCSESSED)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }
                if (transition.Condition.m_Type == ConditionType.AnimationFailed)
                {
                    if (state.m_AnimatingStatus == AnimatingStatus.FAILED)
                    {
                        TransitionToNextNode(abilityEntity, ref state, transition.TargetNodeId);
                        return;
                    }
                }
            }
        }
        private void EvaluateAnimationFrameConditions(Entity abilityEntity, ref AbilityExecutionState state, ExecutionNode currentNode)
        {
            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.m_Type == ConditionType.AnimationFrame)
                {

                    bool isConditionMet = false;

                    isConditionMet = IsCompared(transition.Condition.m_IntValue,
                        state.m_CurrentAnimationFrame, transition.Condition.m_Operator);

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
            if (state.m_DamageDealt == false) { return; }

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.m_Type == ConditionType.DamageDealt)
                {

                    bool isConditionMet = false;

                    isConditionMet = IsCompared(state.m_LastDamageAmount, transition.Condition.m_FloatValue
                        , transition.Condition.m_Operator);

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
            if (state.m_EffectApplied == false) { return; }

            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.m_Type == ConditionType.EffectApplied)
                {
                    if (transition.Condition.m_StringValue == state.m_LastAppliedEffectId)
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


