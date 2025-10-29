using Core.Utilities;
using Cysharp.Threading.Tasks;
using Game;
using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class MoveToCell : IAIJob
    {
        private Entity m_Cell;

        public MoveToCell SetupJob(Entity a_cell)
        {
            m_Cell = a_cell;
            return this;
        }

        public async UniTask DoJob(AIExecutionContext context)
        {
            await G.MoveToCellAsync(context.m_Agent, m_Cell, context.m_World);
        }
    }
}
