using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Domain.Monster.Requests
{
    public struct EnemySpawnRequest : IRequestData{        
        public List<string> EnemiesIDs;
    }
}
