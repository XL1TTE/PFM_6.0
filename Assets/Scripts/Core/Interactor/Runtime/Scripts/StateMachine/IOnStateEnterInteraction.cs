

using Cysharp.Threading.Tasks;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{

    public interface IOnStateEnterInteraction
    {
        UniTask OnStateEnter(Entity a_state);
    }
    public interface IOnStateExitInteraction
    {
        UniTask OnStateExit(Entity a_state);
    }



    public sealed class StopBattleOnPause : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsStateActive<BattleScene>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                BattleECS.Stop();
            }
            return UniTask.CompletedTask;
        }
    }
    public sealed class ContinueBattleOnUnpause : BaseInteraction, IOnStateExitInteraction
    {
        public UniTask OnStateExit(Entity a_state)
        {
            if (SM.IsStateActive<BattleScene>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                BattleECS.Run();
            }
            return UniTask.CompletedTask;
        }
    }

    public sealed class StopMapOnPause : BaseInteraction, IOnStateEnterInteraction
    {
        public UniTask OnStateEnter(Entity a_state)
        {
            if (SM.IsStateActive<MapSceneState>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main_Map.Pause();
            }
            return UniTask.CompletedTask;
        }
    }
    public sealed class ContinueMapOnUnpause : BaseInteraction, IOnStateExitInteraction
    {
        public UniTask OnStateExit(Entity a_state)
        {
            if (SM.IsStateActive<MapSceneState>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main_Map.Unpause();
            }
            return UniTask.CompletedTask;
        }
    }

}
