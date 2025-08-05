using Gameplay.Common.Components;
using Gameplay.Common.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateInitializer : IInitializer
    {
        public World World { get; set; }

        public void OnAwake()
        {
            var stash_battlePlanningState = World.GetStash<BattlePlanningState>();
            
            Entity stateEntity = World.CreateEntity();
            stash_battlePlanningState.Add(stateEntity);

            var req = World.Default.GetRequest<StateChangeRequest>();

            req.Publish(new StateChangeRequest
            {
                NextState = stateEntity
            });
        }

        public void Dispose()
        {

        }
    }
}

