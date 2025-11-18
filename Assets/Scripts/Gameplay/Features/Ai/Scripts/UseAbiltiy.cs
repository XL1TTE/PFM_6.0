using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Abilities.Components;
using Game;
using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class UseAbiltiy : IAIJob
    {
        private AbilityData m_Ability;
        private Entity m_Target;
        public UseAbiltiy(AbilityData a_ability, Entity a_target)
        {
            this.m_Ability = a_ability;
        }

        public async UniTask DoJob(AIExecutionContext context)
        {
            await G.ExecuteAbilityAsync(m_Ability, context.m_Agent, m_Target, context.m_World);
        }
    }

}
