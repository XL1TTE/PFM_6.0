using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using DG.Tweening;
using Domain.AIGraph;
using Domain.BattleField.Tags;
using Domain.Commands.Requests;
using Domain.Extentions;
using Domain.Monster.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AIGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DefaultAgentTargetSelectSystem : ISystem
    {
        public World World { get; set; }

        private Request<AgentTargetSelectionRequest> req_agentTargetSelect;
        private Stash<AIAgentComponent> stash_agentAi;
        private Stash<AIExecutionState> stash_aiState;

        private Stash<TagMonster> stash_monster;
        private Stash<TagOccupiedCell> stash_occupiedCell;

        public void OnAwake()
        {
            req_agentTargetSelect = World.GetRequest<AgentTargetSelectionRequest>();


            stash_agentAi = World.GetStash<AIAgentComponent>();
            stash_aiState = World.GetStash<AIExecutionState>();


            stash_monster = World.GetStash<TagMonster>();
            stash_occupiedCell = World.GetStash<TagOccupiedCell>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_agentTargetSelect.Consume())
            {
                if (isAgentTypeValid(req.AgentEntity))
                {
                    ref var aiState = ref stash_aiState.Get(req.AgentEntity);

                    var targets = PickTargets(req.AgentEntity, req.MaxTargets);

                    if (targets.Count < 1)
                    {
                        aiState.TargetSelectionStatus = AIExecutionState.TargetSelectStatus.NoTargets;
                        aiState.SelectedTargets = default;
                        return;
                    }

                    aiState.TargetSelectionStatus = AIExecutionState.TargetSelectStatus.Successful;
                    aiState.SelectedTargets = targets;
                }
            }
        }
        public void Dispose()
        {

        }

        private List<Entity> PickTargets(Entity agentEntity, int MaxTargets)
        {
            var options = GU.FindAttackOptionsCellsFor(agentEntity, World);

            return options.GetRange(0, Math.Min(MaxTargets, options.Count))
                .Where(cell => isCellValidByOccupier(cell)).Select(
                    cell => stash_occupiedCell.Get(cell).Occupier
                ).ToList();
        }


        private bool isCellValidByOccupier(Entity cell)
        {
            if (stash_occupiedCell.Has(cell) == false) { return false; }
            var occupier = stash_occupiedCell.Get(cell).Occupier;

            if (stash_monster.Has(occupier) == false) { return false; }

            return true;
        }

        private bool isAgentTypeValid(Entity agentEntity)
        {
            if (stash_agentAi.Has(agentEntity) == false) { return false; }
            if (stash_agentAi.Get(agentEntity).Tag != AIType.Default) { return false; }
            return true;
        }
    }
}
