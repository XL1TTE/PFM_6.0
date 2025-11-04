using System.Collections.Generic;
using Domain.BattleField.Components;
using Domain.BattleField.Requests;
using Domain.BattleField.Tags;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.BattleField.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellHoverSystem : ISystem
    {
        public World World { get; set; }

        private Filter _cellsUnderCursor;

        private Request<ChangeCellViewToHoverRequest> req_hoverCell;

        private Stash<CellSpriteLayersComponent> stash_cellSprites;

        private Entity CurrentHoveredCell;


        public void OnAwake()
        {
            _cellsUnderCursor = World.Filter
                .With<UnderCursorComponent>()
                .With<CellSpriteLayersComponent>()
                .With<CellTag>()
                .Build();

            req_hoverCell = World.GetRequest<ChangeCellViewToHoverRequest>();

            stash_cellSprites = World.GetStash<CellSpriteLayersComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_cellsUnderCursor.IsEmpty())
            {
                DisableHighlight();
                return;
            }

            var cell = _cellsUnderCursor.First();
            if (CurrentHoveredCell.Id != cell.Id)
            {
                DisableHighlight();
                EnableHighlight(cell);
            }
        }

        public void Dispose()
        {

        }

        private void DisableHighlight()
        {
            if (CurrentHoveredCell.IsExist())
            {

                req_hoverCell.Publish(new ChangeCellViewToHoverRequest
                {
                    Cells = new List<Entity> { CurrentHoveredCell },
                    State = ChangeCellViewToHoverRequest.HoverState.Disabled
                });

                CurrentHoveredCell = default;
            }
        }

        private void EnableHighlight(Entity cell)
        {
            CurrentHoveredCell = cell;

            req_hoverCell.Publish(new ChangeCellViewToHoverRequest
            {
                Cells = new List<Entity> { cell },
                State = ChangeCellViewToHoverRequest.HoverState.Enabled
            });
        }
    }
}


