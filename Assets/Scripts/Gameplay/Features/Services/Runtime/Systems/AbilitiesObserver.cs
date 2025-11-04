using Domain.Services;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Domain.Notificator;

namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilitiesObserver : ISystem
    {
        public World World { get; set; }

        private Event<ActorActionStatesChanged> evt_ActorActionStateChanged;
        private Stash<ActorActionStatesComponent> stash_ActorActionState;
        private Event<AbilityExecutionStarted> evt_AbilityExecutionStarted;
        private Event<AbilityExecutionEnded> evt_AbilityExecutionEnded;

        public void OnAwake()
        {
            evt_AbilityExecutionStarted = World.GetEvent<AbilityExecutionStarted>();
            evt_AbilityExecutionEnded = World.GetEvent<AbilityExecutionEnded>();
            evt_ActorActionStateChanged = World.GetEvent<ActorActionStatesChanged>();

            stash_ActorActionState = World.GetStash<ActorActionStatesComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_AbilityExecutionStarted.publishedChanges)
            {
                AddActorActionState(evt.m_Caster, ActorActionStates.ExecutingAbility);
            }
            foreach (var evt in evt_AbilityExecutionEnded.publishedChanges)
            {
                RemoveActorActionState(evt.m_Caster, ActorActionStates.ExecutingAbility);
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


