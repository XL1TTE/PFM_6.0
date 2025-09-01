using Domain.ECS;
using Gameplay.BattleField.Systems;
using Scellecs.Morpeh;

namespace Core.ECS.Modules{
    public sealed class GridLogicModule : IWorldModule
    {
        public int Priority => -300;

        public void Initialize(World world)
        {
            var sg_GridLogic = world.CreateSystemsGroup();
            sg_GridLogic.AddSystem(new CellOccupySystem());
            sg_GridLogic.AddSystem(new CellHoverSystem());

            world.AddSystemsGroup(Priority, sg_GridLogic);
        }
    }
}


