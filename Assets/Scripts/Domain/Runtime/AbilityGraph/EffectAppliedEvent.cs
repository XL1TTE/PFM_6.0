using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EffectAppliedEvent: IEventData{
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public Entity EffectEntity;
        public string EffectTemplateId;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ApplyEffectRequest : IRequestData
    {
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public string EffectTemplateId;
    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DamageDealtEvent: IEventData{
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public float BaseDamage;
        public float FinalDamage;
        public DamageType DamageType;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DamageRequest : IRequestData
    {
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public float MinBaseDamage;
        public float MaxBaseDamage;
        public DamageType DamageType;
    }
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimationEvent : IEventData{
        public Entity AnimationTarget;
        public string AnimationName;
        public int CurrentFrameIndex;
        public string EventName;
        public float NormalizedTime;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimationCompleteEvent : IComponent
    {
        public Entity AnimationTarget;
        public string AnimationName;
    }
}

