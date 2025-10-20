using System.Linq;
using Domain.Notificator;
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
    }

}


