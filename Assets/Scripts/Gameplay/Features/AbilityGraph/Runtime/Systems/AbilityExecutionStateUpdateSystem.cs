using System.Collections.Generic;
using Domain.AbilityGraph;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AbilityGraph{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityExecutionStateUpdateSystem : ISystem
    {
        private Filter f_activeAbilities;
        private Event<EffectAppliedEvent> evt_effectApplited;
        private Event<AnimationEvent> evt_animation;
        private Event<DamageDealtEvent> evt_damageDealt;
        private Stash<AbilityExecutionState> stash_abilityExecutionState;
        private Stash<AbilityCasterComponent> stash_abilityCaster;

        public World World { get; set ; }
        
        public void OnAwake()
        {
            f_activeAbilities = World.Filter
                .With<AbilityIsExecutingTag>()
                .With<AbilityExecutionState>()
                .With<AbilityCasterComponent>()
                .Build();


            evt_effectApplited = World.GetEvent<EffectAppliedEvent>();
            evt_animation = World.GetEvent<AnimationEvent>();
            evt_damageDealt = World.GetEvent<DamageDealtEvent>();
            stash_abilityExecutionState = World.GetStash<AbilityExecutionState>();
            stash_abilityCaster = World.GetStash<AbilityCasterComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            ProcessAnimationEvents();
            ProcessDamageEvents();
            ProcessEffectEvents();
        }

        public void Dispose()
        {
        }

        private void ProcessEffectEvents()
        {
            foreach (var evt in evt_effectApplited.publishedChanges)
            {
                if (evt.SourceAbility.isNullOrDisposed(World) == false &&
                    stash_abilityExecutionState.Has(evt.SourceAbility))
                {
                    ref var state = ref stash_abilityExecutionState.Get(evt.SourceAbility);
                    state.LastAppliedEffectId = evt.EffectTemplateId;
                    state.EffectApplied = true;
                }
            }
        }

        private void ProcessDamageEvents()
        {
            foreach (var evt in evt_damageDealt.publishedChanges)
            {
                if (evt.SourceAbility.isNullOrDisposed(World) == false &&
                    stash_abilityExecutionState.Has(evt.SourceAbility))
                {
                    ref var state = ref stash_abilityExecutionState.Get(evt.SourceAbility);
                    
                    state.LastDamageAmount = evt.FinalDamage;
                    state.DamageDealt = true;
                }
            }
        }

        private void ProcessAnimationEvents()
        {
            foreach (var evt in evt_animation.publishedChanges)
            {
                var animRelatedAbilities = new List<Entity>();
                
                foreach(var abt in f_activeAbilities){
                    if(stash_abilityCaster.Get(abt).caster.Id == evt.AnimationTarget.Id){
                        animRelatedAbilities.Add(abt);
                    }
                }

                foreach(var abt in animRelatedAbilities){

                    ref var state = ref stash_abilityExecutionState.Get(abt);

                    state.CurrentAnimationFrame = evt.CurrentFrameIndex;
                    state.LastAnimationEvent = evt.EventName;
                    state.AnimationFrameReached = true;

                }
            }
        }

    }
}
