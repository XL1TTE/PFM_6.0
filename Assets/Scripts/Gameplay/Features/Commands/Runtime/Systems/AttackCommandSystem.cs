using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Commands.Components;
using Domain.Commands.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Commands{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackCommandSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<AttackTargetRequest> req_attackTarget;
        private Stash<IsAttacking> stash_isAttacking;

        public void OnAwake()
        {
            req_attackTarget = World.GetRequest<AttackTargetRequest>();

            stash_isAttacking = World.GetStash<IsAttacking>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_attackTarget.Consume()){

                stash_isAttacking.Add(req.Attacker);
                req.AttackSequence.onComplete += 
                    () => stash_isAttacking.Remove(req.Attacker); 
            }
        }

        public void Dispose()
        {

        }
    }
}


