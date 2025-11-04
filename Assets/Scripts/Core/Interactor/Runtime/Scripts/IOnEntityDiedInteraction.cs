using Cysharp.Threading.Tasks;
using Domain.Extentions;
using Domain.HealthBars.Components;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnEntityDiedInteraction
    {
        UniTask OnEntityDied(
            Entity a_entity,
            Entity a_cause,
            World a_world);
    }


    public sealed class ClearHealthBarsOnDeathInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.NORMAL;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            var stash_healthBarLink = a_world.GetStash<HealthBarLink>();
            if (stash_healthBarLink.Has(a_entity) == false) { return; }

            var t_healthBar = stash_healthBarLink.Get(a_entity).HealthBarEntity;
            if (a_world.TryGetComponent<HealthBarViewLink>(t_healthBar, out var healthBarView))
            {
                healthBarView.Value?.DestorySelf();
            }

            a_world.RemoveEntity(t_healthBar);

            await UniTask.CompletedTask;
        }
    }
}

