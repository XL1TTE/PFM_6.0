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
}
