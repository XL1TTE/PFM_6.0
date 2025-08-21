using System.Collections;
using Core.Utilities;
using Core.Utilities.Extentions;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Common.Systems;
using Scellecs.Morpeh;
using UI.Requests;
using UI.Widgets;
using Unity.IL2CPP.CompilerServices;

namespace Core.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattleState> stash_battleState;



        public void OnAwake()
        {
            evt_onStateEnter = World.GetEvent<OnStateEnterEvent>();

            stash_battleState = World.GetStash<BattleState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (isStateValid(evt.StateEntity))
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

            /* ########################################## */
            /*              Change plate text             */
            /* ########################################## */

            PlateWithText.Instance.Show("Battle stage");

            /* ########################################## */
            /*           Initialize turn system           */
            /* ########################################## */
            
            World.GetRequest<InitializeTurnSystemRequest>().Publish(
                new InitializeTurnSystemRequest{}, true);
        }


        private bool isStateValid(Entity stateEntity)
        {
            if (!stash_battleState.Has(stateEntity)) { return false; }
            else { return true; }
        }

    }

}

