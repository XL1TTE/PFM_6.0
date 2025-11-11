using System;
using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Tags
{
    public interface IEnemySpawnRule
    {
        string GetEnemy();
    }

    [Serializable]
    public sealed class SpawnByWeight : IEnemySpawnRule
    {
        [Serializable]
        struct Rule
        {
            public int m_weight;
            public string m_enemyID;
        }

        [SerializeField] private List<Rule> m_Pool;

        public string GetEnemy()
        {
            int t_totalWeight = 0;
            foreach (var rule in m_Pool)
            {
                t_totalWeight += rule.m_weight;
            }
            var t_pick = UnityEngine.Random.Range(0, t_totalWeight);

            int t_counter = 0;
            foreach (var rule in m_Pool)
            {
                t_counter += rule.m_weight;
                if (t_counter >= t_pick)
                {
                    return rule.m_enemyID;
                }
            }
            return "none";
        }
    }


    [Serializable]
    public struct EnemySpawnPool : IComponent
    {
        [SerializeReference, SubclassSelector]
        public IEnemySpawnRule m_SpawnRule;
    }

}
