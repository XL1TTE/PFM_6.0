using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MapEvStageRequirComponent : IComponent 
{
    public List<STAGES> acceptable_stages;
}

public enum STAGES
{
    VILLAGE,
    CAVE,
    SEAPORT
}