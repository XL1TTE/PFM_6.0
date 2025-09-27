
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
                    new AINode{
                        NodeId  = 0,
                        Type = AINodeType.Immediate,
                        Actions = new List<AIAction>{
                            new AIAction{
                                Type = AIActionType.TryToMove,
                                MoveAnimation = MoveAnimationType.Chess
                            }
                        },
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 1
                            }
                        }
                    },

                    new AINode{
                        NodeId = 1,
                        Type = AINodeType.WaitForMovement,
                        Transitions = new List<AITransition>{
                            new AITransition{
                                TargetNodeId = 2,
                                Condition = new AICondition{
                                    Type = AIConditionType.MoveCompleted
                                }
                            },
                            new AITransition{
                                TargetNodeId = 2,
                                Condition = new AICondition{
                                    Type = AIConditionType.NoAvaibleMoves
                                }
                            },
                        }
                    },
                    new AINode{
                        NodeId = 2,
                        Type = AINodeType.End
                    }
                }
            });
        }
    }
}
