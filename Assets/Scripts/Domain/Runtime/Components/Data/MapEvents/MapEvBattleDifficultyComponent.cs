using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MapEvBattleDifficultyComponent : IComponent 
{
    public BATTLE_DIFFICULTY battle_difficulty;
}

public enum BATTLE_DIFFICULTY
{
    EASY,
    MEDIUM,
    HARD
}