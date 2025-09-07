using Domain.Components;
using Scellecs.Morpeh.Providers;
using UnityEditor;
using UnityEngine;

namespace Domain.CursorDetection.Providers
{
    public sealed class HitBoxComponentProvider : MonoProvider<HitBoxComponent>
    {
        void OnDrawGizmosSelected()
        {
            var data = GetSerializedData();

            Handles.color = Color.green;
            Handles.DrawWireCube((Vector2)transform.position + data.Offset, data.Size);
        }
    }
}


