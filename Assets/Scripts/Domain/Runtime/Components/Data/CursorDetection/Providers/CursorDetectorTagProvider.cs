using Domain.CursorDetection.Components;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Domain.CursorDetection.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorDetectorTagProvider : MonoProvider<TagCursorDetector> 
    {
        [SerializeField] private Color _hitBoxColor = Color.green;
        void OnDrawGizmosSelected()
        {
            Gizmos.color = _hitBoxColor;
            Gizmos.DrawWireSphere(gameObject.transform.position, GetData().DetectionRadius);
        }

    }
}


