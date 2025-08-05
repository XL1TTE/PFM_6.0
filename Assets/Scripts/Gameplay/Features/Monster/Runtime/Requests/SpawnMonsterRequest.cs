using System;
using Scellecs.Morpeh;

namespace Gameplay.Features.Monster.Requests
{
    public struct SpawnNewMonsterRequest : IRequestData{        
        public Entity CellEntity;
    }
}
