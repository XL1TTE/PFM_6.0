// using Domain.Ecs;
// using Gameplay.AbilityGraph;
// using Scellecs.Morpeh;

// namespace Core.ECS.Modules
// {
//     public sealed class AbilityGraphModule : IWorldModule
//     {
//         public int Priority => -75;

//         public void Initialize(World world)
//         {
//             var sg_AbilitiesLogic = world.CreateSystemsGroup();
//             var sg_Cleanup = world.CreateSystemsGroup();

//             sg_AbilitiesLogic.AddSystem(new AbilityActivationSystem());
//             sg_AbilitiesLogic.AddSystem(new AbilityGraphExecutionSystem());
//             sg_AbilitiesLogic.AddSystem(new AbilityExecutionStateUpdateSystem());


//             sg_Cleanup.AddSystem(new AbilityGraphExecutionCleanup());

//             world.AddSystemsGroup(Priority, sg_AbilitiesLogic);
//             world.AddSystemsGroup(Priority + 1, sg_Cleanup);
//         }
//     }
// }


