using System.Collections;
using Core.Utilities;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.TurnSystem.Requests;
using Domain.UI.Mono;
using Domain.UI.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattleState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = StateMachineWorld.Value.GetEvent<OnStateEnterEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattleState>();
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

            /* ########################################## */
            /*              Change plate text             */
            /* ########################################## */

            BattleFieldUIRefs.Instance.InformationBoardWidget.ChangeText("Battle");

            /* ########################################## */
            /*           Initialize turn system           */
            /* ########################################## */
            
            World.GetRequest<InitializeTurnSystemRequest>().Publish(
                new InitializeTurnSystemRequest{}, true);
        }


        private bool IsValid(Entity stateEntity)
        {
            if (!stash_state.Has(stateEntity)) { return false; }
            else { return true; }
        }

    }

}

