
using Domain.DragAndDrop.Requests;
using Domain.DragAndDrop.Tags;
using Domain.Monster.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Monster.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterDragControlSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<ChangeMonsterDraggableStateRequest> _request;
        
        private Filter _monsters;
        
        private Stash<DraggableTag> stash_draggable;

        public void OnAwake()
        {
            _monsters = World.Filter
                .With<TagMonster>().Build();

            stash_draggable = World.GetStash<DraggableTag>();

            _request = World.GetRequest<ChangeMonsterDraggableStateRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _request.Consume())
            {
                if (req.state == ChangeMonsterDraggableStateRequest.State.Enabled)
                {
                    EnableDraggable();
                }
                else if (req.state == ChangeMonsterDraggableStateRequest.State.Disabled)
                {
                    DisableDraggable();
                }
            }
        }

        public void Dispose()
        {

        }

        private void DisableDraggable()
        {
            foreach (var m in _monsters)
            {
                if(stash_draggable.Has(m)){
                    stash_draggable.Remove(m);
                }
            }
        }

        private void EnableDraggable()
        {
            foreach(var m in _monsters){
                stash_draggable.Set(m);
            }
        }
    }

}

