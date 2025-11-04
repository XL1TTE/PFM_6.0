using Domain.Stats.Components;
using Domain.Stats.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameStats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StatsServiceSystem : ISystem
    {

        public World World { get; set; }

        private Stash<CurrentStatsComponent> stash_curStats;

        private Request<ConsumeInteractionRequest> req_ConsumeInteraction;
        private Request<ConsumeMovementRequest> req_ConsumeMovement;

        public void OnAwake()
        {
            stash_curStats = World.GetStash<CurrentStatsComponent>();

            req_ConsumeInteraction = World.GetRequest<ConsumeInteractionRequest>();
            req_ConsumeMovement = World.GetRequest<ConsumeMovementRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_ConsumeInteraction.Consume())
            {
                if (IsHaveStats(req.m_Subject) == false) { continue; }

                stash_curStats.Get(req.m_Subject).m_InteractionActions--;
            }
            foreach (var req in req_ConsumeMovement.Consume())
            {
                if (IsHaveStats(req.m_Subject) == false) { continue; }

                stash_curStats.Get(req.m_Subject).m_MovementActions--;
            }
        }

        public void Dispose()
        {

        }

        private bool IsHaveStats(Entity entity)
        {
            if (stash_curStats.Has(entity) == false) { return false; }
            return true;
        }
    }
}
