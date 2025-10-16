
using Domain.Enemies.Tags;
using Domain.Monster.Tags;
using Domain.Notificator;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActorsActionStateInitializer : ISystem
    {
        private Stash<ActorActionStatesComponent> stash_ActorActionStates;
        private Filter f_Monsters;
        private Filter f_Enemies;

        public World World { get; set; }


        public void OnAwake()
        {
            f_Monsters = World.Filter
                .With<TagMonster>()
                .Without<ActorActionStatesComponent>()
                .Build();
            f_Enemies = World.Filter
                .With<TagEnemy>()
                .Without<ActorActionStatesComponent>()
                .Build();

            stash_ActorActionStates = World.GetStash<ActorActionStatesComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in f_Enemies)
            {
                stash_ActorActionStates.Set(e, new ActorActionStatesComponent { m_Values = new() });
            }
            foreach (var e in f_Monsters)
            {
                stash_ActorActionStates.Set(e, new ActorActionStatesComponent { m_Values = new() });
            }
        }

        public void Dispose()
        {

        }
    }
}


