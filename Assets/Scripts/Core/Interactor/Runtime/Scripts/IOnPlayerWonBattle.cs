using Core.Utilities;
using Cysharp.Threading.Tasks;
using Game;
using Persistence.DS;
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
            string tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_LoseTip", "Parts");

            await GUI.NotifyFullScreenAsync(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Lose", "Parts"), UniTask.WaitForSeconds(3.0f), C.COLOR_LOST_NOTIFICATION, tip_message);
        }

        public async UniTask OnPlayerWon(World a_world)
        {
            // by default the message will be that you have got no rewards
            string tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_WinTipNoRewards", "Parts");

            // if there were rewards, then change the message and state them
            if (BattleReward.battle_rewards.Count > 0)
            {
                tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_WinTip", "Parts");

                
                foreach (var reward in BattleReward.battle_rewards)
                {
                    string reward_name = "";
                    string reward_amount = "";

                    if (reward.Key == "gold") 
                    { reward_name = LocalizationManager.Instance.GetLocalizedValue("General_Gold", "UI_Menu"); }
                    else
                    {


                        reward_name = LocalizationManager.Instance.GetLocalizedValue("General_Gold", "UI_Menu");
                    }

                    tip_message += "\n" + reward_name + " = " + reward_amount;
                }
            }

            await GUI.NotifyFullScreenAsync(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Win", "Parts"), UniTask.WaitForSeconds(3.0f), C.COLOR_WIN_NOTIFICATION);

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
    public sealed class GiveBattleRewards : BaseInteraction, IOnPlayerWonBattle
    {
        public override Priority m_Priority => Priority.NORMAL;
        //public UniTask OnPlayerLost(World a_world)
        //{
        //    LoadingScreen.Instance.LoadScene("MapGeneration");
        //    return UniTask.CompletedTask;
        //}

        public UniTask OnPlayerWon(World a_world)
        {
            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();
            ref var bodyResourcesStorage = ref DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>();

            foreach (var reward in BattleReward.battle_rewards)
            {
                // add gold
                if (reward.Key == "gold")
                {
                    bodyResourcesStorage.gold += reward.Value;
                }
                else
                {
                    // add bodypart if it is already in the storage, or create a new entry with it;
                    if (bodyPartsStorage.parts.ContainsKey(reward.Key))
                    {
                        bodyPartsStorage.parts[reward.Key] += reward.Value;
                    }
                    else
                    {
                        bodyPartsStorage.parts.Add(reward.Key, reward.Value);
                    }
                }
            }

            return UniTask.CompletedTask;
        }
    }
}

