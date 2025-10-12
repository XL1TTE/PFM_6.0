using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MapEvTextChoicesComponent : IComponent 
{
    //    public string string_description;
    //    public string script_result_path;


    /// <summary>
    /// key is the text,
    /// value is the path to result script
    /// </summary>
    public Dictionary<string, IRequestData> choices;
}