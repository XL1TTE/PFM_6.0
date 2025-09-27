using System.Collections.Generic;
using Domain.AbilityGraph;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;

namespace Persistence.DB
{
    public sealed class TestRatAbilityRecord : IDbRecord
    {
        public TestRatAbilityRecord()
        {
            With<ID>(new ID { Value = "abt_TestRat" });
            With<AbilityComponent>();
            With<AbilityTargetsComponent>(new AbilityTargetsComponent
            {
                TargetCount = 1
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
                                Type = ActionType.DealDamage,
                                MinDamageValue = 0,
                                MaxDamageValue = 2,
                                DamageType = DamageType.Physical,
                                TargetIndex = 0
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
                                    Type = ConditionType.DamageDealt,
                                    FloatValue = 0,
                                    Operator = ComparisonOperator.GreaterThen
                                }
                            },
                            new NodeTransition{
                                TargetNodeId = 3,
                                Condition = new TransitionCondition{
                                    Type = ConditionType.DamageDealt,
                                    FloatValue = 0,
                                    Operator = ComparisonOperator.LesserOrEqual
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
                                Type = ActionType.ApplyEffect,
                                OnSelf = false,
                                EffectID = "effect_Empower",
                                EffectDurationInTurns = -1,
                                TargetIndex = 0
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
