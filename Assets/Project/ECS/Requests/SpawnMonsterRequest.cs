using System;
using Scellecs.Morpeh;

namespace ECS.Requests
{
    public struct SpawnNewMonsterRequest : IRequestData{        
        public Entity CellEntity;
    }
}
