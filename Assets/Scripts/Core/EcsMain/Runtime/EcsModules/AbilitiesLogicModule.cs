using Domain.ECS;
using Gameplay.Abilities.Systems;
using Gameplay.TargetSelection.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class AbilitiesLogicModule : IWorldModule
    {
        public int Priority => -90;

        public void Initialize(World world)
        {
            var sg_AbilitiesLogic = world.CreateSystemsGroup();
            sg_AbilitiesLogic.AddSystem(new MoveAbilitySystem());
            sg_AbilitiesLogic.AddSystem(new AttackAbilitySystem());
            sg_AbilitiesLogic.AddSystem(new TargetSelectionSystem());

            world.AddSystemsGroup(Priority, sg_AbilitiesLogic);
        }
    }
}


