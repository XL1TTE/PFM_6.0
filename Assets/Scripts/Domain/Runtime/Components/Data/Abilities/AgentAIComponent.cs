using Project.AI;
using Scellecs.Morpeh;
using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Domain.Abilities.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AgentAIComponent : IComponent
    {
        [SerializeReference, SubclassSelector] public IAIProcessor m_AIModel;
    }
}


