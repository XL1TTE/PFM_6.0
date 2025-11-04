using Cysharp.Threading.Tasks;
using Domain.TurnSystem.Requests;
using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class EndTurn : IAIJob
    {
        public async UniTask DoJob(AIExecutionContext context)
        {
            var t_world = context.m_World;
            var request = t_world.GetRequest<EndTurnRequest>();
            request.Publish(new EndTurnRequest { }, true);

            await UniTask.CompletedTask;
        }
    }
}
