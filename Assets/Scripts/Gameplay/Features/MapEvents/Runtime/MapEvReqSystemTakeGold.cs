
using Domain.CursorDetection.Components;
using Domain.Events;
using Domain.Extentions;
using Domain.Map.Components;
using Domain.MapEvents.Requests;
using Domain.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using System;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;


namespace Gameplay.MapEvents.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MapEvReqSystemTakeGold : ISystem
    {
        public World World { get; set; }

        public Request<TakeGoldRequest> req_take_gold;
        public void OnAwake()
        {
            req_take_gold =
                World.GetRequest<TakeGoldRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_take_gold.Consume())
            {
                ProcessRequest(req);
            }
        }

        public void Dispose()
        {

        }
        private void ProcessRequest(TakeGoldRequest req)
        {
            Debug.Log($"Take gold request is processed, took {req.amount}");
        }
    }

}