using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Services
{
    /// <summary>
    /// Event which notify that entity started executing ability. 
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityExecutionStarted : IEventData
    {
        /// <summary>
        /// Entity that executing the ability.
        /// </summary>
        public Entity m_Caster;
        public Entity m_Ability;
    }
}
