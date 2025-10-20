using System.Collections.Generic;
using Core.Utilities;
using Domain.AbilityGraph;
using Domain.Components;
using Domain.Extentions;
using Domain.Services;

namespace Persistence.DB.Abilities
{
    public sealed class AttackAbility : IDbRecord
    {
        public AttackAbility()
        {
            With<ID>(new ID { Value = "abt_PhysicalAttack" });
            With<AbilityComponent>();
            With<AbilityTargetsComponent>(new AbilityTargetsComponent
            {
                m_TargetCount = 1,
                m_TargetTypes = new List<AbilityTargetType>{
                    AbilityTargetType.ENEMY,
                }
            });
            With<AbilityExecutionGraph>(new AbilityExecutionGraph
            {
                StartNodeId = 0,
                Nodes = new List<ExecutionNode>{
                    // Node 0. 
                    new ExecutionNode{
                        NodeId = 0,
                        Type = NodeType.Immediate,
                        Actions = new List<ActionData>{
                            new ActionData{
                                m_Type = ActionType.PlayTween,
                                m_TweenAnimationCode = TweenAnimations.ATTACK,
                                m_TargetIndex = 0,
                            }
                        },
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{TargetNodeId = 1}
                        }
                    },
                    // Node 1. Waiting for tween interaction frame
                    new ExecutionNode{
                        NodeId = 1,
                        Type = NodeType.WaitForTweenInteractionFrame,
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{
                                TargetNodeId = 2,
                                Condition = new TransitionCondition{
                                    m_Type = ConditionType.TweenInteractionFrame
                                }
                            }
                        }
                    },
                    // Node 2. Deal damage
                    new ExecutionNode{
                        NodeId = 2,
                        Type = NodeType.Immediate,
                        Actions = new List<ActionData>{
                            new ActionData{
                                m_Type = ActionType.DealDamage,
                                m_MinDamageValue = 1,
                                m_MaxDamageValue = 1,
                                m_DamageType = DamageType.Physical,
                                m_TargetIndex = 0
                            }
                        },
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{TargetNodeId = 3}
                        }
                    },
                    // Consume interaction action.    
                    new ExecutionNode{
                        NodeId = 3,
                        Type = NodeType.Immediate,
                        Actions = new List<ActionData>{
                            new ActionData{
                                m_Type = ActionType.ConsumeInteractionAction
                            }
                        },
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{TargetNodeId = 4}
                        }
                    },         
                    // End
                    new ExecutionNode{
                        NodeId = 4,
                        Type = NodeType.End
                    }
                }
            });
        }
    }

}
