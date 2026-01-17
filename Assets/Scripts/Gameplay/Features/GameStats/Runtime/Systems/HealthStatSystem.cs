using Domain.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameStats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthStatSystem : ISystem
    {
        private Filter f_InitCurrHealth;
        private Filter f_InitHealth;
        private Stash<CurrHealth> stash_CurrHealth;
        private Stash<Health> stash_Health;
        private Stash<MaxHealth> stash_MaxHealth;

        public World World { get; set; }


        public void OnAwake()
        {
            f_InitHealth = World.Filter
                .With<MaxHealth>()
                .Without<CurrHealth>()
                .Without<Health>()
                .Build();

            f_InitCurrHealth = World.Filter
                .With<CurrHealth>()
                .Without<Health>()
                .Build();

            stash_CurrHealth = World.GetStash<CurrHealth>();
            stash_Health = World.GetStash<Health>();
            stash_MaxHealth = World.GetStash<MaxHealth>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in f_InitHealth)
            {
                stash_Health.Set(e, new Health(stash_MaxHealth.Get(e).m_Value));
            }
            foreach (var e in f_InitCurrHealth)
            {
                stash_Health.Set(e, new Health(stash_CurrHealth.Get(e).m_Value));
            }
        }

        public void Dispose()
        {

        }
    }
}
