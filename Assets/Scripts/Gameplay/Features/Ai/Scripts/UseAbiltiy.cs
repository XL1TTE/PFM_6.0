using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;
using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class UseAbiltiy : IAIJob
    {
        private Ability m_Ability;
        private Entity m_Target;
        public UseAbiltiy(Ability a_ability, Entity a_target)
        {
            this.m_Ability = a_ability;
        }

        public async UniTask DoJob(AIExecutionContext context)
        {
            await m_Ability.Execute(context.m_Agent, m_Target, context.m_World);
        }
    }

}
