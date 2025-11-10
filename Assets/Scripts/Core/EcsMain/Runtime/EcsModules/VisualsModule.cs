using Domain.ECS;
using Gameplay.BattleField.Systems;
using Gameplay.DragAndDrop.Systems;
using Gameplay.Monster.Systems;
using Gameplay.TurnSystem.Systems;
using Scellecs.Morpeh;
using UI.Systems;

namespace Core.ECS.Modules
{
    public sealed class VisualsModule : IWorldModule
    {
        public int Priority => 100;

        public void Initialize(World world)
        {
            var sg_Visuals = world.CreateSystemsGroup();
            sg_Visuals.AddSystem(new FullScreenNotificationSystem());
            sg_Visuals.AddSystem(new TurnTakerCellMarkSystem());
            sg_Visuals.AddSystem(new InformationBoardViewSystem());
            //sg_Visuals.AddSystem(new TurnQueueRenderSystem());
            sg_Visuals.AddSystem(new TurnTakerAvatarDrawSystem());
            sg_Visuals.AddSystem(new MonsterAbilitiesDrawSystem());
            sg_Visuals.AddSystem(new CellsViewSystem());

            world.AddSystemsGroup(Priority, sg_Visuals);
        }
    }
}


