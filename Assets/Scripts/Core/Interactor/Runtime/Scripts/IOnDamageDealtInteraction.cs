
using Core.Utilities;
using Cysharp.Threading.Tasks;
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
            World a_world,
            int a_Damage);
    }

    public sealed class ShowFloatingDamageInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Source, Entity a_Target, World a_world, int a_Damage)
        {
            a_world.GetEvent<DamageDealtEvent>().NextFrame(new DamageDealtEvent
            {
                m_Source = a_Source,
                m_FinalDamage = a_Damage,
                m_Target = a_Target
            });
            await UniTask.CompletedTask;
        }
    }
    public sealed class UpdateHealthBarInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Source, Entity a_Target, World a_world, int a_Damage)
        {
            await UniTask.Yield();
            GU.UpdateHealthBarValueFor(a_Target, a_world);
        }
    }

    public sealed class CheckIfTargetHaveDiedInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW;

        public async UniTask Execute(Entity a_Source, Entity a_Target, World a_world, int a_Damage)
        {
            var t_targetHealth = GU.GetHealth(a_Target, a_world);
            if (t_targetHealth <= 0)
            {
                await G.DieAsync(a_Target, a_Source, a_world);
            }
        }
    }

}

