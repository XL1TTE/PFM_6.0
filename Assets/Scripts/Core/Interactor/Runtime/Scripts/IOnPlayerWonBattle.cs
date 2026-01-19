using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Monster.Mono;
using Domain.Stats.Components;
using Game;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;

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
        public override Priority m_Priority => Priority.HIGH;
        public async UniTask OnPlayerLost(World a_world)
        {
            string tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_LoseTip", "Battle");

            await GUI.NotifyFullScreenAsync(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Lose", "Battle"), UniTask.WaitForSeconds(3.0f), C.COLOR_LOST_NOTIFICATION, tip_message);
        }

        public async UniTask OnPlayerWon(World a_world)
        {
            // by default the message will be that you have got no rewards
            string tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_WinTipNoRewards", "Battle");

            // if there were rewards, then change the message and state them
            if (BattleReward.battle_rewards.Count > 0)
            {
                tip_message = LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_WinTip", "Battle");

                
                foreach (var reward in BattleReward.battle_rewards)
                {
                    string reward_name = "MISSING NAME";

                    if (reward.Key == "gold") 
                    { reward_name = LocalizationManager.Instance.GetLocalizedValue("General_Gold", "UI_Menu"); }
                    else
                    {
                        if (DataBase.TryFindRecordByID(reward.Key, out var e_record))
                        {
                            if (DataBase.TryGetRecord<Name>(e_record, out var e_name))
                            {
                                reward_name = LocalizationManager.Instance.GetLocalizedValue(e_name.m_Value, "Parts");
                            }
                        }
                    }

                    tip_message += "\n" + reward_name + " = " + reward.Value;
                }
            }

            await GUI.NotifyFullScreenAsync(LocalizationManager.Instance.GetLocalizedValue("Battle_UI_Notification_Win", "Battle"), UniTask.WaitForSeconds(9.0f), C.COLOR_WIN_NOTIFICATION, tip_message);

        }
    }
    public sealed class ChangeMonsterHP : BaseInteraction, IOnPlayerLostBattle, IOnPlayerWonBattle
    {
        public override Priority m_Priority => Priority.HIGH;

        public UniTask OnPlayerLost(World a_world)
        {
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();

            if (crusadeMonsters.Equals(null) || crusadeMonsters.crusade_monsters == null || crusadeMonsters.crusade_monsters.Equals(null)) { return UniTask.CompletedTask; }

            crusadeMonsters.crusade_monsters.Clear();

            RemoveDeadMonstersFromInventoryStorage();
            return UniTask.CompletedTask;
        }

        public UniTask OnPlayerWon(World a_world)
        {
            var f_monsters = GU.GetAllMonstersOnField(a_world);

            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();

            if (crusadeMonsters.Equals(null) || crusadeMonsters.crusade_monsters == null || crusadeMonsters.crusade_monsters.Equals(null)) { return UniTask.CompletedTask; }

            var s_health = a_world.GetStash<Health>();
            var s_name = a_world.GetStash<Name>();

            var tmp_copy = new List<MonsterData>(crusadeMonsters.crusade_monsters);

            foreach (var monst in crusadeMonsters.crusade_monsters)
            {
                bool found_on_field = false;

                foreach (var e in f_monsters)
                {
                    string name = s_name.Get(e).m_Value;
                    if (monst.m_MonsterName == name)
                    {
                        found_on_field = true;

                        if (s_health.Get(e).GetHealth() <= 0)
                        {
                            tmp_copy.Remove(monst);
                        }
                        else
                        {
                            var tmp_monst_copy = monst;

                            tmp_monst_copy.current_hp = s_health.Get(e).GetHealth();

                            tmp_copy.Remove(monst);

                            tmp_copy.Add(tmp_monst_copy);
                        }
                        break;
                    }
                }

                if (!found_on_field)
                {
                    tmp_copy.Remove(monst);
                }
            }

            crusadeMonsters.crusade_monsters = tmp_copy;

            RemoveDeadMonstersFromInventoryStorage();
            return UniTask.CompletedTask;
        }

        private void RemoveDeadMonstersFromInventoryStorage()
        {
            ref var allMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            var tmp_copy = new List<MonsterData>(allMonsters.storage_monsters);

            foreach (var monster in allMonsters.storage_monsters)
            {
                bool tmp_do_not_exist = true;
                foreach (var crusade_monster in crusadeMonsters.crusade_monsters)
                {
                    if (monster.m_MonsterName == crusade_monster.m_MonsterName)
                    {
                        tmp_do_not_exist = false;
                        break;
                    }
                }

                if (tmp_do_not_exist)
                {
                    tmp_copy.Remove(monster);
                }
            }

            allMonsters.storage_monsters = tmp_copy;
        }
    }
    public sealed class LoadMapScene : BaseInteraction, IOnPlayerLostBattle, IOnPlayerWonBattle
    {
        public override Priority m_Priority => Priority.VERY_LOW;
        public UniTask OnPlayerLost(World a_world)
        {
            LoadingScreen.Instance.LoadScene("Laboratory");
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

            BattleReward.battle_rewards.Clear();

            return UniTask.CompletedTask;
        }
    }
}

