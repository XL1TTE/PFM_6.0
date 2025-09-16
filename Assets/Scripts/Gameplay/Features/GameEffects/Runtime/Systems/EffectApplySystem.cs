using System;
using Domain.Extentions;
using Domain.GameEffects;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameEffects
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EffectApplySystem : ISystem
    {
        public World World { get; set; }

        private Request<ApplyEffectRequest> req_applyEffect;
        private Event<EffectAppliedEvent> evt_effectApplied;
        private Stash<EffectsPoolComponent> stash_effectPool;

        public void OnAwake()
        {
            req_applyEffect = World.GetRequest<ApplyEffectRequest>();
            
            evt_effectApplied = World.GetEvent<EffectAppliedEvent>();
            
            stash_effectPool = World.GetStash<EffectsPoolComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_applyEffect.Consume()){
                if(req.EffectId == null){continue;}
                if(req.Target.isNullOrDisposed(World)){continue;}
                
                if(DataBase.TryFindRecordByID(req.EffectId, out var effect_record) == false){continue;}
                
                if(stash_effectPool.Has(req.Target) == false){
                    stash_effectPool.Set(req.Target, new EffectsPoolComponent{
                        StatusEffects = new(),
                        PermanentEffects = new()
                    });
                }
                
                ref var effectPool = ref stash_effectPool.Get(req.Target);
                
                if(req.DurationInTurns == -1){
                    effectPool.PermanentEffects.Add(new PermanentEffect{EffectId = req.EffectId});
                    NotifyEffectApplied(req);
                }
                else if(req.DurationInTurns < 0){
                    throw new ArgumentOutOfRangeException("Effect duration can't be negative. Only -1 is works for permanents effects.");
                }
                else{
                    effectPool.StatusEffects
                        .Add(new StatusEffect{
                           EffectId = req.EffectId,
                           DurationInTurns = req.DurationInTurns,
                           TurnsLeft = req.DurationInTurns 
                        });
                    NotifyEffectApplied(req);
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void NotifyEffectApplied(ApplyEffectRequest req){
            evt_effectApplied.NextFrame(new EffectAppliedEvent{
                Source = req.Source,
                SourceAbility = req.AbilitySource,
                Target = req.Target,
                EffectId = req.EffectId
            });
        }
    }
}
