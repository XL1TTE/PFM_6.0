using System;
using Cysharp.Threading.Tasks;
using Domain.CursorDetection.Components;
using Interactions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace CursorDetection.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorClickDetectionSystem : ISystem
    {
        private Filter f_underCursor;

        public World World { get; set; }


        public void OnAwake()
        {
            f_underCursor = World.Filter.With<UnderCursorComponent>().Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonUp(0))
            {
                foreach (var e in f_underCursor)
                {
                    NotifyPointerClick(e);
                }
            }
        }

        private void NotifyPointerClick(Entity entity)
        {
            Interactor.CallAll<IOnPointerClick>(
                async h => await h.OnPointerClick(entity, World)).Forget();
        }

        public void Dispose()
        {
        }
    }
}

