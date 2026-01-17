using Project;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class BattleReward
    {
        public static Dictionary<string,int> battle_rewards = new Dictionary<string, int>();

        public static void AddRewardsToPool(EnemyLootWrapper[] loot_list)
        {
            if (loot_list == null ||  loot_list.Length == 0) return;

            string id = "";
            int loot_amount_end = 0;
            foreach (var loot in loot_list)
            {
                id = loot.loot_id;
                loot_amount_end = 0;

                // get the amount of rolls that will be made
                // UNUSED, SINCE TOO COMPLICATED
                //int rolls = loot.loot_rolls;
                //if (loot.use_random_rools)
                //{
                //    rolls = Random.Range(loot.loot_min_rolls, loot.loot_max_rolls);
                //}


                // get the amount of loot that will be made from each roll
                int loot_amount = loot.loot_amount;
                if (loot.use_random_amount)
                {
                    loot_amount = Random.Range(loot.loot_min_amount, loot.loot_max_amount + 1);
                }

                // roll for loot with chance check
                // UNUSED, SINCE TOO COMPLICATED
                //for (int i = 0; i < rolls; i++)
                //{
                //    if (Random.Range(0f,1f) <= loot.loot_chance)
                //    {
                //        loot_amount_end += loot_amount;
                //    }
                //}

                // roll for loot with chance check
                if (Random.Range(0f, 1f) <= loot.loot_chance)
                {
                    loot_amount_end += loot_amount;
                }

                // add the loot to total table
                if (loot_amount_end > 0)
                {
                    if (!battle_rewards.ContainsKey(id))
                    {
                        battle_rewards.Add(id, loot_amount_end);
                    }
                    else
                    {
                        battle_rewards[id] += loot_amount_end;
                    }
                }
                continue;
            }
        }
    }
}
