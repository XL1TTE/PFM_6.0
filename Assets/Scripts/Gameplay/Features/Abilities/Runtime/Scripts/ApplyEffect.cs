using System;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.GameEffects;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine.Rendering;

namespace Gameplay.Abilities
{
    public struct ApplyEffect : IAbilityEffect
    {
        /// <summary>
        /// -1 if permanent.
        /// </summary>
        public int m_Duration { get; set; }
        public string m_EffectID { get; set; }

        public bool m_IsSelfCast { get; private set; }

        public ApplyEffect(int a_duration, string m_effectID, bool m_isSelfCast)
        {
            this.m_Duration = a_duration;
            this.m_EffectID = m_effectID;
            this.m_IsSelfCast = m_isSelfCast;
        }

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            if (m_IsSelfCast)
            {
                t_target = t_caster;
            }

            AddEffectToPool(t_target, m_EffectID, m_Duration, t_world);

            foreach (var i in Interactor.GetAll<IOnGameEffectApply>())
            {
                await i.OnEffectApply(m_EffectID, t_target, t_world);
            }
        }

        private void AddEffectToPool(Entity a_subject, string a_effectID, int a_duration, World a_world)
        {
            var effect_pool = a_world.GetStash<EffectsPoolComponent>();
            if (effect_pool.Has(a_subject) == false)
            {
                effect_pool.Set(a_subject, new EffectsPoolComponent
                {
                    m_PermanentEffects = new(),
                    m_StatusEffects = new()
                });
            }

            ref var subjects_pool = ref effect_pool.Get(a_subject);

            if (a_duration <= -1)
            {
                subjects_pool.m_PermanentEffects.Add(new PermanentEffect
                {
                    m_EffectId = a_effectID
                });
            }
            else
            {
                subjects_pool.m_StatusEffects.Add(new StatusEffect
                {
                    m_EffectId = a_effectID,
                    m_DurationInTurns = a_duration,
                    m_TurnsLeft = a_duration
                });

            }
        }
    }
}
