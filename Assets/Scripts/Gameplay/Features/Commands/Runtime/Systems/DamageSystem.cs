using Domain.AbilityGraph;
using Domain.StateMachine.Components;
using Domain.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Commands
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DamageSystem : ISystem
    {
        private Request<DealDamageRequest> req_dealDamageRequest;
        private Event<DamageDealtEvent> evt_damageDealt;
        private Stash<CurrentStatsComponent> stash_currentStats;

        public World World { get; set; }

        public void OnAwake()
        {
            req_dealDamageRequest = World.GetRequest<DealDamageRequest>();
            
            evt_damageDealt = World.GetEvent<DamageDealtEvent>();
            
            stash_currentStats = World.GetStash<CurrentStatsComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_dealDamageRequest.Consume()){
                
                var baseDamage = RollBaseDamage(req.MinBaseDamage, req.MaxBaseDamage);
                var finalDamage = DealDamage(req, baseDamage);
                
                NotifyDamageDealt(req, baseDamage, finalDamage);
            }
        }

        public void Dispose()
        {

        }
        
        private short RollBaseDamage(short min, short max) => (short)Random.Range(min, max + 1);
        
        private void NotifyDamageDealt(DealDamageRequest req, short baseDamage, short finalDamage)
        {
            evt_damageDealt.NextFrame(new DamageDealtEvent
            {
                Source = req.Source,
                Target = req.Target,
                SourceAbility = req.SourceAbility,
                BaseDamage = baseDamage,
                FinalDamage = finalDamage,
                DamageType = req.DamageType
            });
        }

        private short DealDamage(DealDamageRequest req, short baseDamage)
        {
            var target = req.Target;
            var source = req.Source;
            
            if(stash_currentStats.Has(target) == false) {return 0;}
            
            var finalDamage = baseDamage; // Calculate final damage from source damage modifiers and target deffence modifiers;
            
            
            ref var target_stats = ref stash_currentStats.Get(target);
            target_stats.CurrentHealth -= finalDamage;
            
            return finalDamage;
        }
    }
}
