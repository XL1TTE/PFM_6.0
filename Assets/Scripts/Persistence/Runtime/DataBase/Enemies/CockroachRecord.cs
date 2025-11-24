using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class CockroachRecord : IDbRecord
    {
        public CockroachRecord()
        {
            ID("e_Cockroach");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Cockroach_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
