

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
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main.Stop();
            }
            return UniTask.CompletedTask;
        }
    }
    public sealed class ContinueBattleOnUnpause : BaseInteraction, IOnStateExitInteraction
    {
        public UniTask OnStateExit(Entity a_state)
        {
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main.Run();
            }
            return UniTask.CompletedTask;
        }
    }

}
