using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.GameEffects;
using Game;
using Interactions;
using Scellecs.Morpeh;

namespace Gameplay.Abilities
{
    /// <summary>
    /// Removes permanent effect from entity.
    /// </summary>
    public struct RemoveEffect : IAbilityNode
    {
        public string m_EffectID { get; set; }
        public bool m_RemoveFromSelf;

        public RemoveEffect(string a_effectID, bool a_removeFromSelf)
        {
            this.m_EffectID = a_effectID;
            this.m_RemoveFromSelf = a_removeFromSelf;
        }

        public IAbilityNode Clone() => new RemoveEffect(m_EffectID, m_RemoveFromSelf);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            if (m_RemoveFromSelf)
            {
                t_target = t_caster;
            }
            G.Statuses.RemoveEffectFromPool(t_target, m_EffectID, t_world);
            return UniTask.CompletedTask;
        }

    }
}
