using Domain.Components;
using Domain.Providers;
using UnityEngine;

namespace Domain.CursorDetection.Providers
{
    public sealed class HitBoxComponentProvider : ComponentProvider<HitBoxComponent>
    {
        void OnDrawGizmosSelected()
        {
            var data = GetSerializedData();

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((Vector2)transform.position + data.Offset, data.Size);
        }
    }
}


