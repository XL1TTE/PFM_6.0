using System.Collections;
using System.Linq;
using Core.Utilities;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
using Domain.UI.Requests;
using Domain.UI.Widgets;
using Game;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PreBattlePlanningNotificationStateEnterSystem : ISystem
    {
        public World World { get; set; }
        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<PreBattlePlanningNotificationState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = SM.Value.GetEvent<OnStateEnterEvent>();

            stash_state = SM.Value.GetStash<PreBattlePlanningNotificationState>();
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
            Game.GUI.NotifyFullScreen("Planning stage", () => Input.GetMouseButtonDown(0), "Press LMB to continue...");

            SM.ExitState<PreBattlePlanningNotificationState>();
            SM.EnterState<BattlePlanningState>();
        }


        private bool IsValid(Entity stateEntity)
        {
            if (stash_state.Has(stateEntity))
            {
                return true;
            }
            return false;
        }
    }
}

