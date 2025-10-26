using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Stats.Components;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public static partial class G
    {

        /// <summary>
        /// Function try to deal damage to target.
        /// 1) Validate target.
        /// 2) Calculates final damage.
        /// 3) Changes target health.
        /// 4) Sends damage dealt notification. 
        /// </summary>
        /// <param name="a_source">Damage source.</param>
        /// <param name="a_target">Target.</param>
        /// <param name="a_amount">Damage amount.</param>
        /// <param name="a_damageType">Damage type.</param>
        /// <param name="a_world">ECS world in which all affected entities leaves.</param>
        public static void DealDamage(
            Entity a_source,
            Entity a_target,
            int a_amount,
            DamageType a_damageType,
            World a_world)
        {
            DealDamageAsync(a_source, a_target, a_amount, a_damageType, a_world).Forget();
        }
        private static async UniTask DealDamageAsync(
            Entity a_source,
            Entity a_target,
            int a_amount,
            DamageType a_damageType,
            World a_world)
        {
            var t_canTakeDamage = true;
            foreach (var i in Interactor.GetAll<ICanTakeDamageValidator>())
            {
                t_canTakeDamage &= await i.Validate(a_target, a_world);
            }
            if (t_canTakeDamage == false) { return; }

            int t_damageCounter = a_amount;
            foreach (var i in Interactor.GetAll<ICalculateDamageInteraction>())
            {
                t_damageCounter = await i.Execute(
                    a_source, a_target, a_world, a_damageType, t_damageCounter);
            }

            a_world.GetStash<Health>().Get(a_target).ChangeHealth(t_damageCounter);

            // On damage dealt notification
            foreach (var i in Interactor.GetAll<IOnDamageDealtInteraction>())
            {
                await i.Execute(a_source, a_target, a_world, t_damageCounter);
            }
        }

    }
}
