using System;
using System.Collections.Generic;
using Domain.Monster.Mono;
using Scellecs.Morpeh;

namespace Domain.Monster.Requests
{
    public struct SpawnMonstersRequest : IRequestData
    {
        public List<MonsterData> Monsters;
    }
}
