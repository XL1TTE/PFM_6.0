
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactions
{
    public interface IOnPointerEnter
    {
        UniTask OnPointerEnter(Entity entity, World a_world);
    }
    public interface IOnPointerExit
    {
        UniTask OnPointerExit(Entity entity, World a_world);
    }
    public interface IOnPointerClick
    {
        UniTask OnPointerClick(Entity entity, World a_world);
    }


    public sealed class EnemyPointerEventsInteraction
        : BaseInteraction, IOnPointerEnter, IOnPointerExit, IOnPointerClick
    {
        public UniTask OnPointerClick(Entity entity, World a_world)
        {
            Debug.Log($"Clicked {entity.Id}");
            return UniTask.CompletedTask;
        }

        public UniTask OnPointerEnter(Entity entity, World a_world)
        {
            Debug.Log($"Entered {entity.Id}");
            return UniTask.CompletedTask;
        }

        public UniTask OnPointerExit(Entity entity, World a_world)
        {
            Debug.Log($"Exited {entity.Id}");
            return UniTask.CompletedTask;
        }
    }
}
