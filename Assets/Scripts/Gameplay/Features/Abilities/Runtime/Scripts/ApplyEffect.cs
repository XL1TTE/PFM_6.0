using System;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.GameEffects;
using Game;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine.Rendering;

namespace Gameplay.Abilities
{
    public struct ApplyEffect : IAbilityNode
    {
        /// <summary>
        /// -1 if permanent.
        /// </summary>
        public int m_Duration { get; set; }
        public string m_EffectID { get; set; }

        public bool m_IsSelfCast { get; private set; }

        public ApplyEffect(int a_duration, string m_effectID, bool m_isSelfCast = false)
        {
            this.m_Duration = a_duration;
            this.m_EffectID = m_effectID;
            this.m_IsSelfCast = m_isSelfCast;
        }

        public IAbilityNode Clone() => new ApplyEffect(m_Duration, m_EffectID, m_IsSelfCast);

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            if (m_IsSelfCast)
            {
                t_target = t_caster;
            }

            G.Statuses.AddEffectToPool(t_target, m_EffectID, m_Duration, t_world);

            foreach (var i in Interactor.GetAll<IOnGameEffectApply>())
            {
                await i.OnEffectApply(m_EffectID, t_target, t_world);
            }
        }
    }
}
