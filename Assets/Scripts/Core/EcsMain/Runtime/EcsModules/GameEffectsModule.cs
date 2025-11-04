using Domain.ECS;
using Gameplay.GameEffects;
using Scellecs.Morpeh;

namespace Core.ECS.Modules
{
    public sealed class GameEffectsModule : IWorldModule
    {
        public int Priority => -50;

        public void Initialize(World world)
        {
            var sg_GameEffects = world.CreateSystemsGroup();
            sg_GameEffects.AddSystem(new InitializeEffectsFromInitPoolSystem());

            //sg_GameEffects.AddSystem(new StatsFromEffectsCalculationSystem());


            world.AddSystemsGroup(Priority, sg_GameEffects);
        }
    }
}


