using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.GameEffects
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EffectAppliedEvent: IEventData{
        /// <summary>
        /// Can be null if there is no source.
        /// </summary>
        public Entity Source;
        public Entity Target;
        /// <summary>
        /// Should be providen if effect is applied by ability.
        /// </summary>
        public Entity SourceAbility;
        public string EffectId;
    }
}

