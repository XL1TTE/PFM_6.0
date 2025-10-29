using System;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.AbilityGraph;
using Domain.AIGraph;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Persistence.DB;
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
        private Stash<AIAgentComponent> stash_aiAgent;
        private Stash<AIExecutionGraph> stash_aiGraph;
        private Stash<AIExecutionState> stash_aiState;
        private Stash<AIIsExecutingTag> stash_aiActivationTag;
        private Event<NextTurnStartedEvent> evt_nextTurnStarted;

        public void OnAwake()
        {
            f_notInitedAgents = World.Filter
                .With<AIAgentComponent>()
                .Without<AIExecutionGraph>()
                .Without<AIExecutionState>()
                .Build();
            f_agentTurnTaker = World.Filter
                .With<AIAgentComponent>()
                .Without<AIIsExecutingTag>()
                .With<CurrentTurnTakerTag>()
                .Build();

            f_agentCleanup = World.Filter
                .With<AIAgentComponent>()
                .With<AIIsExecutingTag>()
                .Without<CurrentTurnTakerTag>()
                .Build();

            stash_aiAgent = World.GetStash<AIAgentComponent>();
            stash_aiGraph = World.GetStash<AIExecutionGraph>();
            stash_aiState = World.GetStash<AIExecutionState>();
            stash_aiActivationTag = World.GetStash<AIIsExecutingTag>();

            evt_nextTurnStarted = World.GetEvent<NextTurnStartedEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            // foreach (var a in f_notInitedAgents)
            // {
            //     var agentInfo = stash_aiAgent.Get(a);
            //     if (DataBase.TryFindRecordByID(agentInfo.AiGraphID, out var graph_rec) == false)
            //     {
            //         Debug.Log($"{agentInfo.AiGraphID} record not found for ai agent: {a.Id}.");
            //         return;
            //     }

            //     var graph = DataBase.GetRecord<AIExecutionGraph>(graph_rec);

            //     stash_aiGraph.Set(a, graph);
            //     stash_aiState.Set(a, new AIExecutionState());
            // }

            // foreach (var a in f_agentTurnTaker)
            // {
            //     Debug.Log($"Enable agent ai: {a.Id}.");
            //     stash_aiActivationTag.Add(a);
            // }

            // foreach (var a in f_agentCleanup)
            // {
            //     Debug.Log($"Disable agent ai: {a.Id}.");
            //     stash_aiActivationTag.Remove(a);
            //     stash_aiState.Set(a, new AIExecutionState());
            // }

            NewLogic();
        }

        private void NewLogic()
        {
            foreach (var evt in evt_nextTurnStarted.publishedChanges)
            {
                var stash_agentAI = World.GetStash<AgentAIComponent>();
                if (!F.IsEnemy(evt.m_CurrentTurnTaker, World)) { continue; }
                if (stash_agentAI.Has(evt.m_CurrentTurnTaker) == false) { continue; }

                var ai = stash_agentAI.Get(evt.m_CurrentTurnTaker);
                StartAI(ai.m_AIModel, evt.m_CurrentTurnTaker).Forget();
            }
        }

        private async UniTask StartAI(IAIModel ai, Entity agent)
        {
            await ai.Process(agent, World);
        }

        public void Dispose()
        {

        }
    }
}


