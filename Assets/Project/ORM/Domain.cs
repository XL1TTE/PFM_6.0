
using ECS.Components;
using Scellecs.Morpeh;

using UnityEngine;

namespace Project{
    
    public static partial class Domain{
        public static readonly World _ecsWorld = World.Default;

        public static void DeleteEntity(Entity entity)
        {
            _ecsWorld.RemoveEntity(entity);
        }

    }
}
