using System.Collections;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface ICalculateDamageInteraction
    {
        UniTask<int> Execute(
            Entity a_Source,
            Entity a_Target,
            World a_world,
            DamageType a_damageType,
            int a_baseDamage);
    }

    public sealed class CalculateDamageInteraction : BaseInteraction, ICalculateDamageInteraction
    {
        public async UniTask<int> Execute(Entity a_Attacker, Entity a_Target, World a_world, DamageType a_damageType, int a_baseDamage)
        {
            switch (a_damageType)
            {
                case DamageType.PHYSICAL_DAMAGE:
                    return await UniTask.FromResult(
                        CalculatePhysicalDamage(a_Attacker, a_Target, a_world, a_baseDamage));

                case DamageType.FIRE_DAMAGE:
                    return await UniTask.FromResult(
                        CalculatePhysicalDamage(a_Attacker, a_Target, a_world, a_baseDamage));
            }
            return a_baseDamage;
        }

        private int CalculatePhysicalDamage(Entity a_Attacker, Entity a_Target, World a_world, int a_baseDamage)
        {
            return a_baseDamage;
        }
    }
}

