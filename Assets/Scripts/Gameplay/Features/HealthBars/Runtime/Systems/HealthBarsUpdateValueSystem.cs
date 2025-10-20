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
        private Stash<CurrentStatsComponent> stash_curStats;
        private Stash<HealthBarViewLink> stash_healthBarView;
        private Stash<HealthBarOwner> stash_healthBarOwner;

        public World World { get; set; }


        public void OnAwake()
        {
            f_healthBars = World.Filter
                .With<HealthBarTag>()
                .With<HealthBarOwner>()
                .With<HealthBarViewLink>()
                .Build();

            stash_curStats = World.GetStash<CurrentStatsComponent>();
            stash_healthBarView = World.GetStash<HealthBarViewLink>();
            stash_healthBarOwner = World.GetStash<HealthBarOwner>();
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

            if (stash_curStats.Has(owner) == false)
            {
                healthBarView.SetValue(0);
                return;
            }

            var stats = stash_curStats.Get(owner);
            healthBarView.SetValue((float)stats.m_CurrentHealth / stats.m_MaxHealth);
        }

        public void Dispose()
        {

        }
    }
}
