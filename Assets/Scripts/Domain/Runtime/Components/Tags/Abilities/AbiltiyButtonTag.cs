using Domain.Abilities.Components;
using Domain.Abilities.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Tags
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbiltiyButtonTag : IComponent
    {
        public AbilityButtonView m_View;

        /// <summary>
        /// Related ability.
        /// Will be executed on button trigger.
        /// </summary>
        public AbilityData m_Ability;

        /// <summary>
        /// Ability owner entity.
        /// </summary>
        public Entity m_ButtonOwner;
    }

}



