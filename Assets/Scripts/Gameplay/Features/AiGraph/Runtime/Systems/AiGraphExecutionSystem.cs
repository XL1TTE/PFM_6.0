using System;
using System.Collections.Generic;
using Core.Utilities;
using Domain.AIGraph;
using Domain.Commands.Requests;
using Domain.Extentions;
using Domain.TurnSystem.Requests;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AIGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AiGraphExecutionSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_activeAgents;
        private Request<ProcessTurnRequest> req_nextTurn;
        private Filter f_turnTaker;
        private Stash<AIExecutionGraph> stash_aiGraph;
        private Stash<AIExecutionState> stash_aiState;
        private Request<AgentMoveRequest> req_agentMove;

        public void OnAwake()
        {
            f_turnTaker = World.Filter
                .With<CurrentTurnTakerTag>()
                .With<AIAgentComponent>()
                .With<AIIsExecutingTag>()
                .Build();

            f_activeAgents = World.Filter
                .With<AIAgentComponent>()
                .With<AIIsExecutingTag>()
                .With<AIExecutionGraph>()
                .With<AIExecutionState>()
                .Build();

            req_nextTurn = World.GetRequest<ProcessTurnRequest>();

            stash_aiGraph = World.GetStash<AIExecutionGraph>();
            stash_aiState = World.GetStash<AIExecutionState>();

            req_agentMove = World.GetRequest<AgentMoveRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var agent in f_activeAgents)
            {
                ProcessAIExecution(agent, deltaTime);
            }
        }

        private void ProcessAIExecution(Entity agent, float deltaTime)
        {
            var graph = stash_aiGraph.Get(agent);
            ref var state = ref stash_aiState.Get(agent);

            var currentNodeId = state.CurrentNodeId;
            var currentNode = graph.Nodes.Find(n => n.NodeId == currentNodeId);

            if (currentNode == null)
            {
                CompleteExecution();
                return;
            }

            switch (currentNode.Type)
            {
                case AINodeType.Immediate:
                    ExecuteNodeActions(agent, currentNode);
                    if (currentNode.Transitions.Count > 0)
                    {
                        TransitionToNextNode(agent, ref state, currentNode.Transitions[0].TargetNodeId);
                    }
                    else
                    {
                        CompleteExecution();
                    }
                    break;

                case AINodeType.WaitForMovement:
                    EvaluateMovementConditions(agent, ref state, currentNode);
                    break;

                case AINodeType.WaitForAbilityCompletion:
                    //EvaluateAbilityConditions(aiTemplate, ref state, currentNode);
                    break;

                case AINodeType.WaitForCustomCondition:
                    //EvaluateCustomConditions(aiTemplate, ref state, currentNode);
                    break;

                case AINodeType.End:
                    CompleteExecution();
                    break;
            }
        }

        private void ExecuteNodeActions(Entity agent, AINode node)
        {
            ref var state = ref stash_aiState.Get(agent);

            foreach (var action in node.Actions)
            {
                switch (action.Type)
                {
                    case AIActionType.TryToMove:
                        ProcessAgentMovement(agent, action, ref state);
                        break;

                    case AIActionType.SelectAbilityTarget:
                        break;

                    case AIActionType.UseAbility:
                        break;
                }
            }
        }

        private void ProcessAgentMovement(Entity agent, AIAction actionInfo, ref AIExecutionState state)
        {
            req_agentMove.Publish(new AgentMoveRequest
            {
                AgentEntity = agent,
                Aniamtion = actionInfo.MoveAnimation
            });
        }

        private void EvaluateMovementConditions(Entity agent, ref AIExecutionState state, AINode currentNode)
        {
            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.Type == AIConditionType.MoveCompleted)
                {
                    bool isConditionMet = false;

                    isConditionMet = state.MovementStatus == AIExecutionState.MoveStatus.Completed;

                    if (isConditionMet)
                    {
                        TransitionToNextNode(agent, ref state, transition.TargetNodeId);
                        return;
                    }
                }
                if (transition.Condition.Type == AIConditionType.NoAvaibleMoves)
                {
                    bool isConditionMet = false;

                    isConditionMet = state.MovementStatus == AIExecutionState.MoveStatus.NoTargets;

                    if (isConditionMet)
                    {
                        TransitionToNextNode(agent, ref state, transition.TargetNodeId);
                        return;
                    }
                }
            }
        }


        private void TransitionToNextNode(Entity agent, ref AIExecutionState state, int targetNodeId)
        {
            state.CurrentNodeId = targetNodeId;
            state.MovementStatus = AIExecutionState.MoveStatus.None;
            state.AbilityCompleted = false;
            state.CustomConditionMet = false;
        }

        private void CompleteExecution()
        {
            req_nextTurn.Publish(new ProcessTurnRequest());
        }

        public void Dispose() { }
    }
}


