using System;

namespace BattleField.Generator{
    
    [Serializable]
    public class InspectorCell
    {
        public bool isSelected;
    }

    [Serializable]
    public enum FieldType
    {
        standart,
        monster_spawn,
        enemy_spawn
    }
}
