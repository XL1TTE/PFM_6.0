using Domain.Commands.Components;
using Domain.Commands;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Domain.Notificator;

namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovementObserver : ISystem
    {
        public World World { get; set; }
        private Event<MovementStarted> evt_MovementStarted;
        private Event<MovementEnded> evt_MovementEnded;
        private Event<ActorActionStatesChanged> evt_ActorActionStateChanged;
        private Stash<IsMovingTag> stash_TagIsMoving;
        private Stash<ActorActionStatesComponent> stash_ActorActionState;

        public void OnAwake()
        {
            evt_MovementStarted = World.GetEvent<MovementStarted>();
            evt_MovementEnded = World.GetEvent<MovementEnded>();
            evt_ActorActionStateChanged = World.GetEvent<ActorActionStatesChanged>();

            stash_TagIsMoving = World.GetStash<IsMovingTag>();
            stash_ActorActionState = World.GetStash<ActorActionStatesComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_MovementStarted.publishedChanges)
            {
                stash_TagIsMoving.Set(evt.m_Subject, new IsMovingTag { });
                AddActorActionState(evt.m_Subject, ActorActionStates.Moving);
            }
            foreach (var evt in evt_MovementEnded.publishedChanges)
            {
                if (stash_TagIsMoving.Has(evt.m_Subject))
                {
                    stash_TagIsMoving.Remove(evt.m_Subject);
                    RemoveActorActionState(evt.m_Subject, ActorActionStates.Moving);
                }

            }
        }

        private void AddActorActionState(Entity actor, ActorActionStates state)
        {
            ref var aState = ref stash_ActorActionState.Get(actor);

            if (aState.m_Values.Contains(state)) { return; }

            aState.m_Values.Add(state);
            evt_ActorActionStateChanged.NextFrame(new ActorActionStatesChanged
            {
                m_Actor = actor,
                m_Values = aState.m_Values
            });
        }
        private void RemoveActorActionState(Entity actor, ActorActionStates state)
        {
            ref var aState = ref stash_ActorActionState.Get(actor);

            if (aState.m_Values.Contains(state) == false) { return; }

            aState.m_Values.Remove(state);
            evt_ActorActionStateChanged.NextFrame(new ActorActionStatesChanged
            {
                m_Actor = actor,
                m_Values = aState.m_Values
            });

        }

        public void Dispose()
        {

        }

    }
}


