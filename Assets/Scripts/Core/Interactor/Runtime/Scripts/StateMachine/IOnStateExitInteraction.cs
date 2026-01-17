

using Cysharp.Threading.Tasks;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnStateExitInteraction
    {
        UniTask OnStateExit(Entity a_state);
    }

    public sealed class ContinueBattleOnUnpause : BaseInteraction, IOnStateExitInteraction
    {
        public UniTask OnStateExit(Entity a_state)
        {
            if (SM.IsIt<BattleScene>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                BattleECS.Run();
            }
            return UniTask.CompletedTask;
        }
    }


    public sealed class ContinueMapOnUnpause : BaseInteraction, IOnStateExitInteraction
    {
        public UniTask OnStateExit(Entity a_state)
        {
            if (SM.IsIt<MapSceneState>(out _) == false) { return UniTask.CompletedTask; }
            if (SM.IsIt<PauseState>(a_state))
            {
                ECS_Main_Map.Unpause();
            }
            return UniTask.CompletedTask;
        }
    }

    public sealed class ClearPlayerAndEnemyTurnStates : BaseInteraction, IOnTurnEndInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW;

        public UniTask OnTurnEnd(Entity a_turnTaker, World a_world)
        {
            if (SM.IsIt<PlayerTurnState>(out _))
            {
                SM.ExitState<PlayerTurnState>();
            }
            else if (SM.IsIt<EnemyTurnState>(out _))
            {
                SM.ExitState<EnemyTurnState>();
            }
            return UniTask.CompletedTask;
        }


    }

}
