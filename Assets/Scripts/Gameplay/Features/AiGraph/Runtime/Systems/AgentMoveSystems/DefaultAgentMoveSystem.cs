using Core.Utilities;
using DG.Tweening;
using Domain.AIGraph;
using Domain.Services;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AIGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DefaultAgentMoveSystem : ISystem
    {
        public World World { get; set; }

        private Request<AgentMoveRequest> req_agentMove;
        private Request<MoveToCellRequest> req_moveToCellCommand;
        private Stash<AIAgentComponent> stash_agentAi;
        private Stash<AIExecutionState> stash_aiState;

        public void OnAwake()
        {
            req_agentMove = World.GetRequest<AgentMoveRequest>();
            req_moveToCellCommand = World.GetRequest<MoveToCellRequest>();

            stash_agentAi = World.GetStash<AIAgentComponent>();
            stash_aiState = World.GetStash<AIExecutionState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_agentMove.Consume())
            {
                if (isAgentTypeValid(req.AgentEntity))
                {
                    ref var aiState = ref stash_aiState.Get(req.AgentEntity);

                    var target = PickMoveTarget(req.AgentEntity);

                    if (target.isNullOrDisposed(World))
                    {
                        aiState.MovementStatus = AIExecutionState.MoveStatus.NoTargets;
                        return;
                    }

                    switch (req.Aniamtion)
                    {
                        case MoveAnimationType.Chess:
                            aiState.MovementStatus = AIExecutionState.MoveStatus.Animating;
                            ProcessChessMove(req.AgentEntity, target);
                            break;
                        case MoveAnimationType.Linear:
                            break;
                    }
                }
            }
        }
        public void Dispose()
        {

        }

        private Entity PickMoveTarget(Entity agentEntity)
        {
            var moveOpts = GU.FindMoveOptionsCellsFor(agentEntity, World);
            if (moveOpts.Count > 0)
            {
                return moveOpts[0];
            }
            return default;
        }


        private void ProcessChessMove(Entity agentEntity, Entity target)
        {
            var sequence = A.ChessMovement(agentEntity, target, World);
            sequence.OnComplete(() =>
            {
                CompleteMovement(agentEntity);
            });

            req_moveToCellCommand.Publish(new MoveToCellRequest
            {
                m_MoveSequence = sequence,
                m_Subject = agentEntity,
                m_TargetCell = target
            });
        }

        private void CompleteMovement(Entity agentEntity)
        {
            stash_aiState.Get(agentEntity).MovementStatus = AIExecutionState.MoveStatus.Completed;
        }

        private bool isAgentTypeValid(Entity agentEntity)
        {
            if (stash_agentAi.Has(agentEntity) == false) { return false; }
            if (stash_agentAi.Get(agentEntity).Tag != AIType.Default) { return false; }
            return true;
        }
    }
}
