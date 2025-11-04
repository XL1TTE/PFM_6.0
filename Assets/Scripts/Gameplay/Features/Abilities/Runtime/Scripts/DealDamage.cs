using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Requests;
using Game;
using Interactions;
using Persistence.DB;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class DealDamage : IAbilityNode
    {
        public int m_BaseDamage { get; set; }
        public DamageType m_DamageType { get; set; }

        public DealDamage(int a_baseDamage, DamageType a_damageType)
        {
            this.m_BaseDamage = a_baseDamage;
            this.m_DamageType = a_damageType;
        }

        public IAbilityNode Clone() => new DealDamage(m_BaseDamage, m_DamageType);

        public async UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            bool t_canTakeDamage = true;
            foreach (var i in Interactor.GetAll<ICanTakeDamageValidator>())
            {
                t_canTakeDamage &= await i.Validate(t_target, t_world);
            }
            if (t_canTakeDamage == false) { return; }

            int t_damageCounter = m_BaseDamage;
            foreach (var i in Interactor.GetAll<ICalculateDamageInteraction>())
            {
                t_damageCounter = await i.Execute(
                    t_caster, t_target, t_world, m_DamageType, t_damageCounter);
            }

            G.DealDamage(t_caster, t_target, t_damageCounter, m_DamageType, t_world);
        }
    }

}
