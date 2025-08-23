using System;
using Scellecs.Morpeh;

namespace Domain.Monster.Requests
{
    public struct SpawnNewMonsterRequest : IRequestData{        
        public Entity CellEntity;
    }
}
