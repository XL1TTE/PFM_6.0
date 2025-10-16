
using System.Collections.Generic;
using Domain.AIGraph;
using Domain.Components;

namespace Persistence.DB
{
    public class AiVillageRatRecord : MonsterPartRecord
    {
        public AiVillageRatRecord()
        {
            With<ID>(new ID { Value = "ai_RatEnemy" });
            With<AIExecutionGraph>(new AIExecutionGraph
            {
                Nodes = new List<AINode>
                {
                    new AINode{ // Try to find attack target
                        NodeId  = 0,
                        Type = AINodeType.Immediate,
                        Actions = new List<AIAction>{
                            new AIAction{
                                Type = AIActionType.SelectTarget,
                                MaxTargets = 1
                            }
                        },
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 1
                            }
                        }
                    },

                    new AINode{ // Check if any valid target was found
                        NodeId  = 1,
                        Type = AINodeType.WaitForTargetSelection,
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 2, // to attack node
                                Condition = new AICondition{
                                    Type = AIConditionType.TargetsSelected
                                }
                            },
                            new AITransition{
                                TargetNodeId = 4, // to move node
                                Condition = new AICondition{
                                    Type = AIConditionType.NoAvaibleTargets
                                }
                            }
                        }
                    },

                    new AINode{ // Attack if target was found
                        NodeId  = 2,
                        Type = AINodeType.Immediate,
                        Actions = new List<AIAction>{
                            new AIAction{
                                Type = AIActionType.UseAbility,
                                AbilityId = "abt_PhysicalAttack"
                            }
                        },
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 3,
                            }
                        }
                    },
                    new AINode{ // Wait for abiltiy to complete
                        NodeId  = 3,
                        Type = AINodeType.WaitForAbilityExecutionCompleted,
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 4,
                                Condition = new AICondition{
                                    Type = AIConditionType.AbiltiyUseCompleted
                                }
                            },
                            new AITransition{
                                TargetNodeId = 4,
                                Condition = new AICondition{
                                    Type = AIConditionType.AbiltiyCastFailed
                                }
                            },
                        }
                    },

                    new AINode{ // Try to move
                        NodeId  = 4,
                        Type = AINodeType.Immediate,
                        Actions = new List<AIAction>{
                            new AIAction{
                                Type = AIActionType.TryToMove,
                                MoveAnimation = MoveAnimationType.Chess
                            }
                        },
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 5
                            }
                        }
                    },

                    new AINode{ // Wait for move to complete
                        NodeId = 5,
                        Type = AINodeType.WaitForMovement,
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 6,
                                Condition = new AICondition{
                                    Type = AIConditionType.MoveCompleted
                                }
                            },
                            new AITransition{
                                TargetNodeId = 6,
                                Condition = new AICondition{
                                    Type = AIConditionType.NoAvaibleMoves
                                }
                            },
                        }
                    },
                    new AINode{
                        NodeId = 6,
                        Type = AINodeType.End
                    }
                }
            });
        }
    }
}
