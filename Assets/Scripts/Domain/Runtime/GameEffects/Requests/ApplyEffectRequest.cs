using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Domain.GameEffects
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ApplyEffectRequest : IRequestData
    {
        public Entity Target;
        /// <summary>
        /// Can be null if there is no source.
        /// </summary>
        public Entity Source;
        /// <summary>
        /// Should be providen if effect is applied by ability.
        /// </summary>
        public Entity AbilitySource;
        public string EffectId;
        
        /// <summary>
        /// Provide -1, if effect is permanent.
        /// </summary>
        public short DurationInTurns;
    }
}
