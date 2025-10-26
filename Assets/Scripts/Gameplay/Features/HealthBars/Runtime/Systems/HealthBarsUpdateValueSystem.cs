using System;
using Domain.BattleField.Events;
using Domain.HealthBars.Components;
using Domain.HealthBars.Requests;
using Domain.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.HealthBars.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthBarsUpdateValueSystem : ISystem
    {
        private Filter f_healthBars;
        private Stash<HealthBarViewLink> stash_healthBarView;
        private Stash<HealthBarOwner> stash_healthBarOwner;

        private Stash<Health> stash_Health;
        private Stash<MaxHealth> stash_MaxHealth;

        public World World { get; set; }


        public void OnAwake()
        {
            f_healthBars = World.Filter
                .With<HealthBarTag>()
                .With<HealthBarOwner>()
                .With<HealthBarViewLink>()
                .Build();

            stash_healthBarView = World.GetStash<HealthBarViewLink>();
            stash_healthBarOwner = World.GetStash<HealthBarOwner>();


            stash_Health = World.GetStash<Health>();
            stash_MaxHealth = World.GetStash<MaxHealth>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in f_healthBars)
            {
                UpdateHealthBarValue(e);
            }
        }

        private void UpdateHealthBarValue(Entity e)
        {
            var owner = stash_healthBarOwner.Get(e).Value;
            ref var healthBarView = ref stash_healthBarView.Get(e).Value;

            if (stash_Health.Has(owner) == false || stash_MaxHealth.Has(owner) == false)
            {
                healthBarView.SetValue(0);
                return;
            }

            var health = stash_Health.Get(owner).m_Value;
            var max_health = stash_MaxHealth.Get(owner).m_Value;

            healthBarView.SetValue((float)health / max_health);
        }

        public void Dispose()
        {

        }
    }
}
