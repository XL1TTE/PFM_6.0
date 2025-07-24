using System;
using Scellecs.Morpeh;


namespace ECS.Requests{
    public struct MoveMonsterRequest : IRequestData{
        public Entity entityID;

        public float target_x;
        public float target_y;
    }
}
