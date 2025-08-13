using Core.Components;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Core.Providers{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class prov_TagCursorDetector : MonoProvider<TagCursorDetector> 
    {
        [SerializeField] private Color _hitBoxColor = Color.green;
        void OnDrawGizmosSelected()
        {
            Gizmos.color = _hitBoxColor;
            Gizmos.DrawSphere(gameObject.transform.position, GetData().DetectionRadius);
        }

    }
}


