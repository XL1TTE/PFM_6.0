using Cysharp.Threading.Tasks;
using Domain.TurnSystem.Requests;
using Game;
using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class EndTurn : IAIJob
    {
        public async UniTask DoJob(AIExecutionContext context)
        {
            var t_world = context.m_World;

            G.NextTurn(t_world);

            await UniTask.CompletedTask;
        }
    }
}
