using Domain.GameEffects;
using Interactions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameEffects
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializeEffectsFromInitPoolSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_toInit;
        private Stash<InitialEffectsPoolComponent> stash_InitEffectsPool;
        private Stash<EffectsPoolComponent> stash_EffectsPool;

        public void OnAwake()
        {
            f_toInit = World.Filter
                .With<InitialEffectsPoolComponent>()
                .Build();

            stash_InitEffectsPool = World.GetStash<InitialEffectsPoolComponent>();
            stash_EffectsPool = World.GetStash<EffectsPoolComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in f_toInit)
            {
                var Ipool = stash_InitEffectsPool.Get(entity);
                stash_EffectsPool.Set(entity, new EffectsPoolComponent
                {
                    m_PermanentEffects = Ipool.m_PermanentEffects,
                    m_StatusEffects = Ipool.m_TemporalEffects,
                });
                stash_InitEffectsPool.Remove(entity);

                foreach (var effect in Ipool.m_PermanentEffects)
                {
                    foreach (var i in Interactor.GetAll<IOnGameEffectApply>())
                    {
                        i.OnEffectApply(effect.m_EffectId, entity, World);
                    }
                }
                foreach (var effect in Ipool.m_TemporalEffects)
                {
                    foreach (var i in Interactor.GetAll<IOnGameEffectApply>())
                    {
                        i.OnEffectApply(effect.m_EffectId, entity, World);
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
