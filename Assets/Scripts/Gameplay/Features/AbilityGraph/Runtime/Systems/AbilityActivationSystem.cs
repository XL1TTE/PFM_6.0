using Domain.AbilityGraph;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AbilityGraph{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityActivationSystem : ISystem
    {
        private Request<AbilityUseRequest> req_activateAbility;
        private Stash<AbilityTargetsComponent> stash_abilityTargets;
        private Stash<AbilityCasterComponent> stash_abilityCaster;
        private Stash<AbilityComponent> stash_ability;
        private Stash<AbilityExecutionState> stash_abilityExecutionState;
        private Stash<AbilityExecutionGraph> stash_abilityExecutionGraph;
        private Stash<AbilityIsExecutingTag> stash_abilityIsExecutingTag;

        public World World { get; set ; }

        public void OnAwake()
        {
            req_activateAbility = World.GetRequest<AbilityUseRequest>();
            
            stash_abilityTargets = World.GetStash<AbilityTargetsComponent>();
            stash_abilityCaster = World.GetStash<AbilityCasterComponent>();
            stash_ability = World.GetStash<AbilityComponent>();
            stash_abilityExecutionState = World.GetStash<AbilityExecutionState>();
            stash_abilityExecutionGraph = World.GetStash<AbilityExecutionGraph>();
            stash_abilityIsExecutingTag = World.GetStash<AbilityIsExecutingTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_activateAbility.Consume())
            {
                var abilityCopy = CreateAbilityCopy(req.AbilityTemplate);

                stash_abilityCaster.Set(abilityCopy, new AbilityCasterComponent
                {
                    caster = req.Caster
                });

                ref var targetsComponent = ref stash_abilityTargets.Get(abilityCopy);
                targetsComponent.targets = req.Targets.ToArray();

                stash_abilityIsExecutingTag.Set(abilityCopy, new AbilityIsExecutingTag());
            }
        }
        
        public void Dispose()
        {
        }
        
        
        private Entity CreateAbilityCopy(Entity abilityTemplate){
            var abilityCopy = World.CreateEntity();
            
            DataBase.TryGetRecord<AbilityComponent>(abilityTemplate, out var ability);
            DataBase.TryGetRecord<AbilityExecutionGraph>(abilityTemplate, out var graph);
            DataBase.TryGetRecord<AbilityExecutionState>(abilityTemplate, out var state);
            DataBase.TryGetRecord<AbilityTargetsComponent>(abilityTemplate, out var targets);
            
            stash_ability.Set(abilityCopy, ability);
            stash_abilityExecutionState.Set(abilityCopy, state);
            stash_abilityExecutionGraph.Set(abilityCopy, graph);
            stash_abilityTargets.Set(abilityCopy, targets);
            
            return abilityCopy;
        }
    }
}
