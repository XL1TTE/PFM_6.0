using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.DragAndDrop.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.DragAndDrop.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragInputSystem : ISystem
    {
        public World World { get; set; }

        private Filter _underCursor;

        private bool isClicked = false;

        private Request<StartDragRequest> req_startDrag;
        private Request<EndDragRequest> req_endDrag;

        private Stash<UnderCursorComponent> stash_underCursor;

        public void OnAwake()
        {
            _underCursor = World.Filter.With<UnderCursorComponent>().Build();

            req_startDrag = World.GetRequest<StartDragRequest>();
            req_endDrag = World.GetRequest<EndDragRequest>();

            stash_underCursor = World.GetStash<UnderCursorComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isClicked)
                {

                    foreach (var entity in _underCursor)
                    {
                        req_startDrag.Publish(new StartDragRequest
                        {
                            DraggedEntity = entity,
                            ClickWorldPos = stash_underCursor.Get(entity).HitPoint
                        });
                        isClicked = true;
                        break;
                    }
                }
                else
                {
                    req_endDrag.Publish(new EndDragRequest { });
                    isClicked = false;
                }
            }

        }

        public void Dispose()
        {

        }
    }
}


