using System;
using UnityEngine;

namespace BattleField.Generator{
    
    [Serializable]
    public class InspectorCell
    {
        public bool isSelected;
        public GameObject CellPrefab;
    }

    [Serializable]
    public enum FieldType
    {
        standart,
        monster_spawn,
        enemy_spawn
    }
}
