using Domain.AbilityGraph;
using Domain.AIGraph;
using Domain.Services;
using Domain.TurnSystem.Tags;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.AIGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AiAgentStateUpdateSystem : ISystem
    {
        public World World { get; set; }

        private Event<AbilityExecutionEnded> evt_AbilityExecutionEnded;
        private Stash<AIExecutionState> stash_aiState;

        public void OnAwake()
        {
            evt_AbilityExecutionEnded = World.GetEvent<AbilityExecutionEnded>();

            stash_aiState = World.GetStash<AIExecutionState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_AbilityExecutionEnded.publishedChanges)
            {
                if (stash_aiState.Has(evt.m_Caster) == false) { return; }
                SetAgentAbilityExectuionStatusToCompleted(evt.m_Caster);
            }
        }

        private void SetAgentAbilityExectuionStatusToCompleted(Entity abilityEntity)
        {
            stash_aiState.Get(abilityEntity).AbilityExecutionStatus = AIExecutionState.AbilityExecuteStatus.Completed;
        }

        public void Dispose()
        {

        }
    }
}


