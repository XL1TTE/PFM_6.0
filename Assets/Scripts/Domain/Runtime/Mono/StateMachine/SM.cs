using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Gameplay.StateMachine.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.StateMachine.Mono
{


    public static class SM
    {
        static SM()
        {
            m_World = World.Create();

            evt_onStateEnter = m_World.GetEvent<OnStateEnterEvent>();
            evt_onStateExit = m_World.GetEvent<OnStateExitEvent>();

            var systems = m_World.CreateSystemsGroup();
            systems.AddSystem(new StateExitCleanupSystem());
            m_World.AddSystemsGroup(0, systems);
        }

        public static World m_World;

        private static Event<OnStateEnterEvent> evt_onStateEnter;
        private static Event<OnStateExitEvent> evt_onStateExit;

        private static void CommitChanges()
        {
            m_World.Commit();
        }

        public static void Update()
        {
            m_World.Update(Time.deltaTime);
            m_World.CleanupUpdate(Time.deltaTime);
        }

        public static bool IsStateActive<T>(out T state) where T : struct, IState
        {
            var states = m_World.Filter.With<T>().Build();
            if (states.IsEmpty()) { state = default; return false; }

            state = m_World.GetStash<T>().Get(states.First());
            return true;
        }

        public static bool IsStateActiveOptimized<T>(Filter stateFilter, Stash<T> stash, out T state) where T : struct, IState
        {
            if (stateFilter.IsEmpty()) { state = default; return false; }
            if (stash.Has(stateFilter.First()) == false)
            {
                state = default;
                return false;
            }
            state = stash.Get(stateFilter.First());
            return true;
        }

        public static Entity EnterState<T>() where T : struct, IState
        {
            var state = m_World.CreateEntity();

            var stash_smState = m_World.GetStash<StateMachineState>();
            stash_smState.Add(state);

            var stash = m_World.GetStash<T>();
            stash.Add(state);

            evt_onStateEnter.NextFrame(new OnStateEnterEvent { StateEntity = state });

            CommitChanges();
            return state;
        }

        public static void ExitState<T>() where T : struct, IState
        {
            var states = m_World.Filter.With<T>().Build();
            if (states.IsEmpty()) { return; }

            var state = states.First();

            evt_onStateExit.NextFrame(new OnStateExitEvent { StateEntity = state });
        }

        /// <summary>
        /// Forbbiden to use.
        /// </summary>
        /// <param name="stateEntity"></param>
        public static void RemoveState(Entity stateEntity)
        {
            if (m_World.Has(stateEntity))
            {
                m_World.RemoveEntity(stateEntity);
            }
            CommitChanges();
        }


        public static void Dispose()
        {
            foreach (var e in m_World.Filter.With<StateMachineState>().Build())
            {
                m_World.RemoveEntity(e);
            }
        }
    }

}
