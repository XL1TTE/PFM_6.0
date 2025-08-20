using Core.Components;
using Core.Utilities.Extentions;
using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEditor;
using UnityEngine;

public sealed class ColliderComponentProvider : MonoProvider<ColliderComponent>
{
    void OnDrawGizmosSelected()
    {
        var data = GetSerializedData();
        
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireCube(data.offset, data.size);
    }
}
