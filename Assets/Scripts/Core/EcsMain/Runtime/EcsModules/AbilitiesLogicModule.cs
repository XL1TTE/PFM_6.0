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
            //sg_AbilitiesLogic.AddSystem(new MoveAbilityButtonSystem());
            sg_AbilitiesLogic.AddSystem(new AbilityButtonSystem());
            sg_AbilitiesLogic.AddSystem(new TargetSelectionSystem());

            world.AddSystemsGroup(Priority, sg_AbilitiesLogic);
        }
    }
}


