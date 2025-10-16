using Domain.AbilityGraph;
using Domain.Commands;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.AbilityGraph
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityActivationSystem : ISystem
    {
        private Request<AbilityActivateRequest> req_activateAbility;
        private Stash<AbilityTargetsComponent> stash_abilityTargets;
        private Stash<AbilityCasterComponent> stash_abilityCaster;
        private Stash<AbilityComponent> stash_ability;
        private Stash<AbilityExecutionState> stash_abilityExecutionState;
        private Stash<AbilityExecutionGraph> stash_abilityExecutionGraph;
        private Stash<AbilityIsExecutingTag> stash_abilityIsExecutingTag;
        private Event<AbilityActivated> evt_AbilityActivated;

        public World World { get; set; }

        public void OnAwake()
        {
            req_activateAbility = World.GetRequest<AbilityActivateRequest>();

            stash_abilityTargets = World.GetStash<AbilityTargetsComponent>();
            stash_abilityCaster = World.GetStash<AbilityCasterComponent>();
            stash_ability = World.GetStash<AbilityComponent>();
            stash_abilityExecutionState = World.GetStash<AbilityExecutionState>();
            stash_abilityExecutionGraph = World.GetStash<AbilityExecutionGraph>();
            stash_abilityIsExecutingTag = World.GetStash<AbilityIsExecutingTag>();

            evt_AbilityActivated = World.GetEvent<AbilityActivated>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_activateAbility.Consume())
            {
                if (DataBase.TryFindRecordByID(req.AbilityTemplateID, out var AbilityTemplate) == false)
                {
                    throw new System.Exception($"Ability with id: {req.AbilityTemplateID} was not found!");
                }

                var abilityCopy = CreateAbilityCopy(AbilityTemplate);

                SetupAbilityMeta(abilityCopy, req);

                ActivateAbility(abilityCopy);

            }
        }

        public void Dispose()
        {
        }

        private void SetupAbilityMeta(Entity ability, AbilityActivateRequest req)
        {
            stash_abilityCaster.Set(ability, new AbilityCasterComponent
            {
                caster = req.Caster
            });

            ref var targetsComponent = ref stash_abilityTargets.Get(ability);
            targetsComponent.targets = req.Targets.ToArray();
        }

        private void ActivateAbility(Entity ability)
        {
            stash_abilityIsExecutingTag.Set(ability, new AbilityIsExecutingTag());
            evt_AbilityActivated.NextFrame(new AbilityActivated
            {
                m_Ability = ability
            });
        }

        private Entity CreateAbilityCopy(Entity abilityTemplate)
        {
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
