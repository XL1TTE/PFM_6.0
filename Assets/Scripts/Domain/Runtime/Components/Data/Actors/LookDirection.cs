using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

public enum Directions : byte
{
    LEFT,
    RIGHT
}

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct LookDirection : IComponent
{
    public Directions m_Value;
}
