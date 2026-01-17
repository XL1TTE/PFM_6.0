

using Cysharp.Threading.Tasks;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.UI.Mono;
using Game;
using Scellecs.Morpeh;
using UnityEngine;

namespace Interactions
{

    public interface IOnStateEnterInteraction
    {
        UniTask OnStateEnter(Entity a_state);
    }

    public sealed class ShowBookInfoOnBattleState : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsIt<BattleState>(a_state) == false) { return UniTask.CompletedTask; }

            BattleUiRefs.Instance.BookWidget.ShowTurnTakerInfo();
            return UniTask.CompletedTask;
        }
    }

    public sealed class HideBookInfoOnBattleLoad : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {

            if (SM.IsIt<BattleSceneInitializeState>(a_state) == false) { return UniTask.CompletedTask; }

            BattleUiRefs.Instance.BookWidget.HideTurnTakerInfo();
            BattleUiRefs.Instance.BookWidget.HideHoveredEntityInfo();

            return UniTask.CompletedTask;
        }
    }

    public sealed class StopBattleOnPause : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsIt<BattleScene>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                BattleECS.Stop();
            }
            return UniTask.CompletedTask;
        }
    }


    public sealed class StopMapOnPause : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsIt<MapSceneState>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main_Map.Pause();
            }
            return UniTask.CompletedTask;
        }
    }

    public sealed class UpdateBattleBoardText : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsIt<PlayerTurnState>(a_state))
            {
                BattleUiRefs.Instance.InformationBoardWidget.ChangeText("Player's Turn");
                return UniTask.CompletedTask;
            }

            else if (SM.IsIt<EnemyTurnState>(a_state))
            {
                BattleUiRefs.Instance.InformationBoardWidget.ChangeText("Enemy's Turn");
                return UniTask.CompletedTask;
            }

            return UniTask.CompletedTask;
        }
    }

}
