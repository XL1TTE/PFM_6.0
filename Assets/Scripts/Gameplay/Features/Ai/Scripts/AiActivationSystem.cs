using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Project.AI;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.AIGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AiActivationSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_notInitedAgents;
        private Filter f_agentTurnTaker;
        private Filter f_agentCleanup;
        private Stash<AgentAICancellationToken> stash_aiCancell;
        private Event<NextTurnStartedEvent> evt_nextTurnStarted;

        public void OnAwake()
        {
            stash_aiCancell = World.GetStash<AgentAICancellationToken>();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_nextTurnStarted.publishedChanges)
            {
                var stash_agentAI = World.GetStash<AgentAIComponent>();

                if (stash_agentAI.Has(evt.m_CurrentTurnTaker) == false) { continue; }

                var ai = stash_agentAI.Get(evt.m_CurrentTurnTaker);
                Debug.Log("Start AI.");
                StartAI(ai.m_AIModel, evt.m_CurrentTurnTaker).Forget();
            }
        }

        private async UniTask StartAI(IAIProcessor ai, Entity agent)
        {
            await ai.Process(agent, World);
        }

        public void Dispose()
        {

        }
    }
}


