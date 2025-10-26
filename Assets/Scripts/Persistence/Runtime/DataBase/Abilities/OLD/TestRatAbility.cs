using System.Collections.Generic;
using Domain.AbilityGraph;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;

namespace Persistence.DB.Abilities
{
    public sealed class TestRatAbilityRecord : IDbRecord
    {
        public TestRatAbilityRecord()
        {
            ID("abt_TestRat");

            With<ID>(new ID { Value = "abt_TestRat" });
            With<AbilityComponent>();
            With<AbilityTargetsComponent>(new AbilityTargetsComponent
            {
                m_TargetCount = 1
            });
            With<AbilityExecutionGraph>(new AbilityExecutionGraph
            {
                StartNodeId = 0,
                Nodes = new List<ExecutionNode>{
                    // Node 0. First attack
                    new ExecutionNode{
                        NodeId = 0,
                        Type = NodeType.Immediate,
                        Actions = new List<ActionData>{
                            new ActionData{
                                m_Type = ActionType.DealDamage,
                                m_MinDamageValue = 0,
                                m_MaxDamageValue = 2,
                                m_DamageType = DamageType.Physical,
                                m_TargetIndex = 0
                            }
                        },
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{TargetNodeId = 1}
                        }
                    },
                    // Node 1. Damage dealt condition
                    new ExecutionNode{
                        NodeId = 1,
                        Type = NodeType.WaitForDamage,
                        Actions = new List<ActionData>{},
                        Transitions = new List<NodeTransition>{
                            new NodeTransition{
                                TargetNodeId = 2,
                                Condition = new TransitionCondition{
                                    m_Type = ConditionType.DamageDealt,
                                    m_FloatValue = 0,
                                    m_Operator = ComparisonOperator.GreaterThen
                                }
                            },
                            new NodeTransition{
                                TargetNodeId = 3,
                                Condition = new TransitionCondition{
                                    m_Type = ConditionType.DamageDealt,
                                    m_FloatValue = 0,
                                    m_Operator = ComparisonOperator.LesserOrEqual
                                }
                            }
                        }
                    },
                    // Node 2. Apply effect if any damage dealt
                    new ExecutionNode{
                        NodeId = 2,
                        Type = NodeType.Immediate,
                        Actions = new List<ActionData>{
                            new ActionData{
                                m_Type = ActionType.ApplyEffect,
                                m_OnSelf = false,
                                m_EffectID = "effect_Empower",
                                m_EffectDurationInTurns = -1,
                                m_TargetIndex = 0
                            }
                        },
                        Transitions = new List<NodeTransition>{new NodeTransition{TargetNodeId = 3}}
                    },
                    // End
                    new ExecutionNode{
                        NodeId = 3,
                        Type = NodeType.End
                    }
                }
            });
        }
    }

}
