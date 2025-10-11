using Domain.HealthBars.Components;
using Domain.HealthBars.Requests;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.HealthBars.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthBarManageSystem : ISystem
    {
        public World World { get; set; }

        private Stash<HealthBarLink> stash_healthBarLink;
        private Stash<HealthBarRenderer> stash_healthBarRenderer;
        private Stash<HealthBarOwner> stash_healthBarOwner;
        private Stash<HealthBarViewLink> stash_healthBarViewLink;
        private Stash<HealthBarTag> stash_healthBarTag;
        private Stash<IHaveHealthBar> stash_iHaveHealthBar;
        private Transform m_HealthBarsSceneConteiner;
        private Request<CreateHealthBarRequest> req_createHealthBar;
        private Request<ChangeHealthBarRendererRequest> req_changeHealthBarRenderer;

        public void OnAwake()
        {
            m_HealthBarsSceneConteiner = new GameObject("[HEALTH_BARS]").transform;

            req_createHealthBar = World.GetRequest<CreateHealthBarRequest>();
            req_changeHealthBarRenderer = World.GetRequest<ChangeHealthBarRendererRequest>();

            stash_healthBarLink = World.GetStash<HealthBarLink>();
            stash_healthBarRenderer = World.GetStash<HealthBarRenderer>();
            stash_healthBarOwner = World.GetStash<HealthBarOwner>();
            stash_healthBarViewLink = World.GetStash<HealthBarViewLink>();
            stash_healthBarTag = World.GetStash<HealthBarTag>();
            stash_iHaveHealthBar = World.GetStash<IHaveHealthBar>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_createHealthBar.Consume())
            {
                if (IsHealthBarCouldBeCreated(req.Owner, req.Renderer) == false) { continue; }
                CreateHealthBar(req.Owner, req.Renderer);
            }
            foreach (var req in req_changeHealthBarRenderer.Consume())
            {
                ChangeHealthBarPosition(req.Owner, req.NewRenderer);
            }
        }

        private Entity CreateHealthBar(Entity owner, Entity renderer)
        {
            var pos = GetHealthBarRendererWorldPosition(renderer);
            var prefab = stash_iHaveHealthBar.Get(owner).HealthBarPrefab;

            var view = CreateHealthBarView(prefab, pos);

            var entity = CreateHealthBarEntity();
            LinkHealthBarEntityWithView(entity, view);
            LinkHealthBarWithOwner(owner, entity);
            return entity;
        }

        private bool IsHealthBarCouldBeCreated(Entity owner, Entity renderer)
        {
            if (stash_iHaveHealthBar.Has(owner) == false) { return false; }
            if (stash_healthBarRenderer.Has(renderer) == false) { return false; }
            return true;
        }

        private Vector3 GetHealthBarRendererWorldPosition(Entity renderer)
        {
            return stash_healthBarRenderer.Get(renderer).GetHealthBarWorldPosition();
        }

        private HealthBarView CreateHealthBarView(HealthBarView prefab, Vector3 worldPosition)
        {
            return UnityEngine.Object.Instantiate(prefab,
                worldPosition, new Quaternion(), m_HealthBarsSceneConteiner);
        }

        private Entity CreateHealthBarEntity()
        {
            var entity = World.CreateEntity();
            stash_healthBarTag.Set(entity, new HealthBarTag { });
            return entity;
        }


        private void LinkHealthBarWithOwner(Entity owner, Entity healthBar)
        {
            stash_healthBarOwner.Set(healthBar,
                new HealthBarOwner { Value = owner });
            stash_healthBarLink.Set(owner,
                new HealthBarLink { HealthBarEntity = healthBar });
        }

        private void LinkHealthBarEntityWithView(Entity healthBar, HealthBarView view)
        {
            stash_healthBarViewLink.Set(healthBar,
                new HealthBarViewLink { Value = view });
        }

        private void ChangeHealthBarPosition(Entity owner, Entity newRenderer)
        {
            if (stash_healthBarLink.Has(owner) == false) { return; }
            if (stash_healthBarRenderer.Has(newRenderer) == false) { return; }

            var newPos = GetHealthBarRendererWorldPosition(newRenderer);

            var _healthBarEntity = stash_healthBarLink.Get(owner).HealthBarEntity;
            ref var view = ref stash_healthBarViewLink.Get(_healthBarEntity);
            view.Value.SetPosition(newPos);
        }

        public void Dispose()
        {
        }
    }
}
