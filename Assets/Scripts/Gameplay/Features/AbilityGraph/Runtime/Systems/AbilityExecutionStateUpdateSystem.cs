// using System.Collections.Generic;
// using Core.Utilities;
// using Domain.AbilityGraph;
// using Domain.Extentions;
// using Domain.GameEffects;
// using Domain.Services;
// using Scellecs.Morpeh;
// using Unity.IL2CPP.CompilerServices;

// namespace Gameplay.AbilityGraph
// {
//     [Il2CppSetOption(Option.NullChecks, false)]
//     [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
//     [Il2CppSetOption(Option.DivideByZeroChecks, false)]
//     public sealed class AbilityExecutionStateUpdateSystem : ISystem
//     {
//         private Filter f_activeAbilities;
//         private Event<EffectAppliedEvent> evt_EffectApplited;
//         private Event<DamageDealtEvent> evt_DamageDealt;
//         private Event<AnimatingEnded> evt_AnimatingEnded;
//         private Event<TweenInteractionFrameRiched> evt_TweenInteractionFrameRiched;
//         private Stash<AbilityExecutionState> stash_AbilityExecutionState;
//         private Stash<AbilityCasterComponent> stash_AbilityCaster;

//         public World World { get; set; }

//         public void OnAwake()
//         {
//             f_activeAbilities = World.Filter
//                 .With<AbilityIsExecutingTag>()
//                 .With<AbilityExecutionState>()
//                 .With<AbilityCasterComponent>()
//                 .Build();


//             evt_EffectApplited = World.GetEvent<EffectAppliedEvent>();
//             evt_DamageDealt = World.GetEvent<DamageDealtEvent>();

//             evt_AnimatingEnded = World.GetEvent<AnimatingEnded>();
//             evt_TweenInteractionFrameRiched = World.GetEvent<TweenInteractionFrameRiched>();

//             stash_AbilityExecutionState = World.GetStash<AbilityExecutionState>();
//             stash_AbilityCaster = World.GetStash<AbilityCasterComponent>();
//         }

//         public void OnUpdate(float deltaTime)
//         {
//             ProcessAnimationEvents();
//             ProcessDamageEvents();
//             ProcessEffectEvents();
//         }

//         public void Dispose()
//         {
//         }

//         private void ProcessEffectEvents()
//         {
//             foreach (var evt in evt_EffectApplited.publishedChanges)
//             {
//                 if (evt.SourceAbility.isNullOrDisposed(World) == false &&
//                     stash_AbilityExecutionState.Has(evt.SourceAbility))
//                 {
//                     ref var state = ref stash_AbilityExecutionState.Get(evt.SourceAbility);
//                     state.m_LastAppliedEffectId = evt.EffectId;
//                     state.m_EffectApplied = true;
//                 }
//             }
//         }

//         private void ProcessDamageEvents()
//         {
//             foreach (var evt in evt_DamageDealt.publishedChanges)
//             {
//                 if (evt.SourceAbility.isNullOrDisposed(World) == false &&
//                     stash_AbilityExecutionState.Has(evt.SourceAbility))
//                 {
//                     ref var state = ref stash_AbilityExecutionState.Get(evt.SourceAbility);

//                     state.m_LastDamageAmount = evt.FinalDamage;
//                     state.m_DamageDealt = true;
//                 }
//             }
//         }

//         private void ProcessAnimationEvents()
//         {
//             foreach (var evt in evt_AnimatingEnded.publishedChanges)
//             {
//                 var subject = evt.m_Subject;
//                 foreach (var e in f_activeAbilities)
//                 {
//                     if (stash_AbilityCaster.Get(e).caster.Id == subject.Id)
//                     {
//                         stash_AbilityExecutionState.Get(e).m_AnimatingStatus = evt.m_AnimatingStatus;
//                     }
//                 }
//             }

//             foreach (var evt in evt_TweenInteractionFrameRiched.publishedChanges)
//             {
//                 var subject = evt.m_Subject;
//                 foreach (var e in f_activeAbilities)
//                 {
//                     if (stash_AbilityCaster.Get(e).caster.Id == subject.Id)
//                     {
//                         stash_AbilityExecutionState.Get(e).m_IsTweenInteractionFrame = true;
//                     }
//                 }
//             }
//         }

//     }
// }
