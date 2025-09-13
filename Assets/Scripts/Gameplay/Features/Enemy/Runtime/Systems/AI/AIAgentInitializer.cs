using System.Collections.Generic;
using System.Linq;
using Domain.Abilities.Components;
using Domain.AI.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Commands.Requests;
using Domain.Enemies.Tags;
using Domain.Monster.Tags;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Enemies
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AIAgentInitializer : IInitializer
    {
        public World World { get; set; }
        
        public void OnAwake()
        {
            var f_aiAgents = World.Filter
                .With<AIAgentType>()
                .Build();

            var stash_aiAgentType = World.GetStash<AIAgentType>();
            
            var stash_defaultAi = World.GetStash<DefaultAIAgentStateComponent>();

            foreach (var agent in f_aiAgents){
                var agentType = stash_aiAgentType.Get(agent).Value;
                switch(agentType){
                    case AIAgentType.AgentType.Default:
                        stash_defaultAi.Set(agent, new DefaultAIAgentStateComponent{
                            Value = DefaultAIAgentStateComponent.AgentState.Idle});
                        break;
                    case AIAgentType.AgentType.Tank:
                        break;
                    case AIAgentType.AgentType.Healer:
                        break;
                }
            }
        }
        public void Dispose()
        {

        }
        
    }
}

