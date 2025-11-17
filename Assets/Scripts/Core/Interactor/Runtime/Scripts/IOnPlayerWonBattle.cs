using Core.Utilities;
using Cysharp.Threading.Tasks;
using Game;
using Scellecs.Morpeh;
using Unity.VisualScripting;

namespace Interactions
{
    public interface IOnPlayerWonBattle
    {
        UniTask OnPlayerWon(World a_world);
    }
    public interface IOnPlayerLostBattle
    {
        UniTask OnPlayerLost(World a_world);
    }


    public sealed class ShowLoseNotification : BaseInteraction, IOnPlayerLostBattle, IOnPlayerWonBattle
    {
        public override Priority m_Priority => Priority.NORMAL;
        public async UniTask OnPlayerLost(World a_world)
        {
            await GUI.NotifyFullScreenAsync("You have lost!", UniTask.WaitForSeconds(3.0f), C.COLOR_LOST_NOTIFICATION);
        }

        public async UniTask OnPlayerWon(World a_world)
        {
            await GUI.NotifyFullScreenAsync("You have won!", UniTask.WaitForSeconds(3.0f), C.COLOR_WIN_NOTIFICATION);

        }
    }
    public sealed class LoadMapScene : BaseInteraction, IOnPlayerLostBattle, IOnPlayerWonBattle
    {
        public override Priority m_Priority => Priority.VERY_LOW;
        public UniTask OnPlayerLost(World a_world)
        {
            LoadingScreen.Instance.LoadScene("MapGeneration");
            return UniTask.CompletedTask;
        }

        public UniTask OnPlayerWon(World a_world)
        {
            LoadingScreen.Instance.LoadScene("MapGeneration");
            return UniTask.CompletedTask;
        }
    }
}

