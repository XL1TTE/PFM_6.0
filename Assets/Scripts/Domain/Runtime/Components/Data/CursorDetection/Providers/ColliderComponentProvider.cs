using Domain.CursorDetection.Components;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Domain.CursorDetection.Providers
{
    public sealed class ColliderComponentProvider : MonoProvider<ColliderComponent>
    {
        void OnDrawGizmosSelected()
        {
            var data = GetSerializedData();

            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(data.offset, data.size);
        }
    }
}


