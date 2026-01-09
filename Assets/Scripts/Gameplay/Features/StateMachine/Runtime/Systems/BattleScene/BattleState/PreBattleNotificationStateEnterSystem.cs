using System.Collections;
using Core.Utilities;
using Cysharp.Threading.Tasks;
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
using UnityEngine;

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
            evt_onStateEnter = SM.m_World.GetEvent<OnStateEnterEvent>();

            stash_state = SM.m_World.GetStash<PreBattleNotificationState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Enter(evt.StateEntity).Forget();
                }
            }

        }

        public void Dispose()
        {

        }

        private async UniTask Enter(Entity stateEntity)
        {
            BattleUiRefs.Instance.InformationBoardWidget.ChangeText(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_State_Battle", "Battle"));

            await Game.GUI.NotifyFullScreenAsync(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Battle", "Battle"),
                UniTask.WaitUntil(() => Input.GetMouseButtonDown(0)), C.COLOR_NOTIFICATION_BG_DEFAULT, LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Continue", "Battle"));

            AudioManager.Instance.PlaySound(AudioManager.buttonClickSound);
            SM.ExitState<PreBattleNotificationState>();
            SM.EnterState<BattleState>();
        }

        private bool IsValid(Entity stateEntity)
        {
            if (!stash_state.Has(stateEntity)) { return false; }
            else { return true; }
        }

    }

}

