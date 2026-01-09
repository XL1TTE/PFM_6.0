using System.Collections;
using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
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
using Unity.VisualScripting;
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
            evt_onStateEnter = SM.m_World.GetEvent<OnStateEnterEvent>();

            stash_state = SM.m_World.GetStash<PreBattlePlanningNotificationState>();
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
            Game.GUI.NotifyFullScreen(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Plan", "Battle"),
                UniTask.WaitUntil(() => Input.GetMouseButtonDown(0)), C.COLOR_NOTIFICATION_BG_DEFAULT, LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Continue", "Battle"));

            AudioManager.Instance.PlaySound(AudioManager.buttonClickSound);
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

