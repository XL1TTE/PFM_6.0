using System.Linq;
using Domain.GameEffects;
using Domain.Notificator;
using Domain.Stats.Components;
using Scellecs.Morpeh;

namespace Core.Utilities
{
    public static class V
    {
        public static bool IsActorBusy(Entity actor, World world)
        {
            var actorStates = world.GetStash<ActorActionStatesComponent>();
            return actorStates.Get(actor).m_Values.Any(s =>
            s == ActorActionStates.Moving |
            s == ActorActionStates.ExecutingAbility |
            s == ActorActionStates.Animating);
        }


        public static bool IsBleeding(Entity a_actor, World a_world)
            => a_world.GetStash<BleedingStatusComponent>().Has(a_actor);
        public static bool IsPoisoned(Entity a_actor, World a_world)
            => a_world.GetStash<PoisonStatusComponent>().Has(a_actor);
        public static bool IsBuring(Entity a_actor, World a_world)
            => a_world.GetStash<BurningStatusComponent>().Has(a_actor);

    }

}


