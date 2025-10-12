using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MapEvBattleSizeComponent : IComponent 
{
    public BATTLE_SIZE battle_size;
}

public enum BATTLE_SIZE
{
    SMALL,
    BIG, 
    MEDIUM
}