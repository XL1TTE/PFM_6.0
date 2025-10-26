using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.GameEffects;
using Interactions;
using Scellecs.Morpeh;

namespace Gameplay.Abilities
{
    /// <summary>
    /// Removes permanent effect from entity.
    /// </summary>
    public struct RemoveEffect : IAbilityEffect
    {
        public string m_EffectID { get; set; }
        public bool m_RemoveFromSelf;

        public RemoveEffect(string a_effectID, bool a_removeFromSelf)
        {
            this.m_EffectID = a_effectID;
            this.m_RemoveFromSelf = a_removeFromSelf;
        }

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            if (m_RemoveFromSelf)
            {
                t_target = t_caster;
            }

            if (TryRemoveEffectFromPool(t_target, m_EffectID, t_world))
            {
                foreach (var i in Interactor.GetAll<IOnGameEffectRemove>())
                {
                    await i.OnEffectRemove(m_EffectID, t_target, t_world);
                }
            }
        }

        private bool TryRemoveEffectFromPool(Entity a_subject, string a_EffectID, World a_world)
        {
            var effect_pool = a_world.GetStash<EffectsPoolComponent>();
            if (effect_pool.Has(a_subject))
            {
                ref var pool = ref effect_pool.Get(a_subject);
                for (int i = 0; i < pool.m_PermanentEffects.Count; ++i)
                {
                    if (pool.m_PermanentEffects[i].m_EffectId == a_EffectID)
                    {
                        pool.m_PermanentEffects.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
