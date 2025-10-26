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
        public AbiltiyButtonView m_View;

        /// <summary>
        /// Id of related ability in data base.
        /// Will be executed on button trigger.
        /// </summary>
        public string m_AbilityID;

        /// <summary>
        /// Ability owner entity.
        /// </summary>
        public Entity m_ButtonOwner;
    }

}



