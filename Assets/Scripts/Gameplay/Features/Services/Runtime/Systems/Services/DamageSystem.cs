// using Domain.AbilityGraph;
// using Domain.Events;
// using Domain.Requests;
// using Domain.Stats.Components;
// using Scellecs.Morpeh;
// using Unity.IL2CPP.CompilerServices;
// using UnityEngine;

// namespace Gameplay.Commands
// {
//     [Il2CppSetOption(Option.NullChecks, false)]
//     [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
//     [Il2CppSetOption(Option.DivideByZeroChecks, false)]
//     public sealed class DamageSeviceSystem : ISystem
//     {
//         private Request<DealDamageRequest> req_dealDamageRequest;
//         private Event<DamageDealtEvent> evt_damageDealt;
//         private Stash<Health> stash_Health;

//         public World World { get; set; }

//         public void OnAwake()
//         {
//             req_dealDamageRequest = World.GetRequest<DealDamageRequest>();

//             evt_damageDealt = World.GetEvent<DamageDealtEvent>();

//             stash_Health = World.GetStash<Health>();
//         }

//         public void OnUpdate(float deltaTime)
//         {
//             foreach (var req in req_dealDamageRequest.Consume())
//             {

//                 var damage = req.m_Damage;
//                 DealDamage(req, damage);

//                 NotifyDamageDealt(req, damage);
//             }
//         }

//         public void Dispose()
//         {

//         }

//         private short RollBaseDamage(short min, short max) => (short)Random.Range(min, max + 1);

//         private void NotifyDamageDealt(DealDamageRequest req, int damageDealt)
//         {
//             evt_damageDealt.NextFrame(new DamageDealtEvent
//             {
//                 m_Source = req.m_Source,
//                 m_Target = req.m_Target,
//                 m_FinalDamage = damageDealt,
//                 m_DamageType = req.m_DamageType,
//             });
//         }

//         private void DealDamage(DealDamageRequest req, int damage)
//         {
//             var target = req.m_Target;
//             var source = req.m_Source;

//             if (stash_Health.Has(target) == false) { return; }

//             ref var target_health = ref stash_Health.Get(target);
//             target_health.m_Value -= damage;
//         }
//     }
// }
