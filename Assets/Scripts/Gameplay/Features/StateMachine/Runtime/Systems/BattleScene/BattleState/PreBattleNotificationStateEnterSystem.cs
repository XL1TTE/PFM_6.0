using System.Collections;
using Core.Utilities;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.TurnSystem.Requests;
using Domain.UI.Mono;
using Domain.UI.Requests;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PreBattleNotificationStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<PreBattleNotificationState> stash_state;



        public void OnAwake()
        {
            evt_onStateEnter = StateMachineWorld.Value.GetEvent<OnStateEnterEvent>();

            stash_state = StateMachineWorld.Value.GetStash<PreBattleNotificationState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Enter(evt.StateEntity);
                }
            }

        }

        public void Dispose()
        {

        }

        private void Enter(Entity stateEntity)
        {
            RellayCoroutiner.Run(EnterRoutine(stateEntity));
        }

        private IEnumerator EnterRoutine(Entity stateEntity)
        {
            World.GetRequest<FullScreenNotificationRequest>().Publish(
            new FullScreenNotificationRequest
            {
                state = FullScreenNotificationRequest.State.Enable,
                Message = "Battle stage",
                Tip = "Press LMB to continue..."
            }, true);

            yield return new WaitForMouseDown(0);

            World.GetRequest<FullScreenNotificationRequest>().Publish(
            new FullScreenNotificationRequest
            {
                state = FullScreenNotificationRequest.State.Disable,
            }, true);
                
            StateMachineWorld.ExitState<PreBattleNotificationState>();
            StateMachineWorld.EnterState<BattleState>();
        }


        private bool IsValid(Entity stateEntity)
        {
            if (!stash_state.Has(stateEntity)) { return false; }
            else { return true; }
        }

    }

}

