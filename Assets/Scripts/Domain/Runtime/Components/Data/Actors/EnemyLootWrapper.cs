using System;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class EnemyLootWrapper
    {
        //public bool use_fixed_rolls = false;
        //public uint loot_rolls = 0;

        //public bool use_random_rools = false;
        //public uint loot_min_rolls = 0;
        //public uint loot_max_rolls = 0;

        //public string loot_id = string.Empty;

        //public bool use_fixed_amount = false;
        //public uint loot_amount = 0;

        //public bool use_random_amount = false;
        //public uint loot_min_amount = 0;
        //public uint loot_max_amount = 0;

        //// should be 0 < loot_chance < 1;
        //public float loot_chance = 0f;


        [Header("ID of loot to spawn")]
        public string loot_id;


        // UNUSED, SINCE TOO COMPLICATED
        // This specifies the amount of rolls that will be made
        //[Space(1)]
        //[Header("Rolls values")]
        //public bool use_fixed_rolls = true;
        //public int loot_rolls = 1;

        //public bool use_random_rools = false;
        //public int loot_min_rolls;
        //public int loot_max_rolls;


        // Each time a roll succedes the check - the amount will be added to the total
        [Space(1)]
        [Header("Amount values")]
        public bool use_fixed_amount = true;
        public int loot_amount;

        public bool use_random_amount = false;
        public int loot_min_amount;
        public int loot_max_amount;


        // should be 0 < loot_chance < 1;
        [Range(0.0f, 1.0f)]
        public float loot_chance;
    }
}
