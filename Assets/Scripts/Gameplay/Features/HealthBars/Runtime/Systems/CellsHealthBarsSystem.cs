
using Domain.BattleField.Events;
using Domain.HealthBars.Components;
using Domain.HealthBars.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.HealthBars.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellsHealthBarsSystem : ISystem
    {
        public World World { get; set; }

        private Request<CreateHealthBarRequest> req_createHealthBar;
        private Request<ChangeHealthBarRendererRequest> req_changeHealthBarRenderer;
        private Event<EntityCellPositionChangedEvent> evt_cellPosChanged;
        private Stash<HealthBarLink> stash_healthBarLink;

        public void OnAwake()
        {
            req_createHealthBar = World.GetRequest<CreateHealthBarRequest>();
            req_changeHealthBarRenderer = World.GetRequest<ChangeHealthBarRendererRequest>();

            evt_cellPosChanged = World.GetEvent<EntityCellPositionChangedEvent>();

            stash_healthBarLink = World.GetStash<HealthBarLink>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_cellPosChanged.publishedChanges)
            {
                if (stash_healthBarLink.Has(evt.m_Subject) == false)
                {
                    RequestHealthBarCreation(evt.m_Subject, evt.m_nCell);
                    continue;
                }
                RequestUpdateHealthBarPosition(evt.m_Subject, evt.m_nCell);
            }
        }

        private void RequestHealthBarCreation(Entity owner, Entity renderer)
        {
            req_createHealthBar.Publish(new CreateHealthBarRequest
            {
                Owner = owner,
                Renderer = renderer
            });
        }

        private void RequestUpdateHealthBarPosition(Entity owner, Entity newRenderer)
        {
            req_changeHealthBarRenderer.Publish(new ChangeHealthBarRendererRequest
            {
                Owner = owner,
                NewRenderer = newRenderer
            });
        }

        public void Dispose()
        {

        }
    }
}
