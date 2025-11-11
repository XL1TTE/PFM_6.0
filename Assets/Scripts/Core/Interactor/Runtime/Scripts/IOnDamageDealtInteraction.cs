
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Domain.Events;
using Domain.HealthBars.Components;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnDamageDealtInteraction
    {
        UniTask Execute(
            Entity a_Source,
            Entity a_Target,
            DamageType a_damageType,
            World a_world,
            int a_Damage);
    }

    public sealed class ShowFloatingDamageInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Source, Entity a_Target, DamageType a_damageType, World a_world, int a_Damage)
        {
            GUI.ShowFloatingDamage(a_Target, a_Damage, a_damageType, a_world);
            await UniTask.CompletedTask;
        }
    }
    public sealed class UpdateHealthBarInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Source, Entity a_Target, DamageType a_damageType, World a_world, int a_Damage)
        {
            await UniTask.Yield();
            GU.UpdateHealthBarValueFor(a_Target, a_world);
        }
    }

    public sealed class CheckIfTargetHaveDiedInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW;

        public async UniTask Execute(Entity a_Source, Entity a_Target, DamageType a_damageType, World a_world, int a_Damage)
        {
            if (F.IsDead(a_Target, a_world)) { return; }

            var t_targetHealth = GU.GetHealth(a_Target, a_world);
            if (t_targetHealth <= 0)
            {
                await G.DieAsync(a_Target, a_Source, a_world);
            }
        }
    }

}

