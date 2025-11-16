using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class RatRecord : IDbRecord
    {
        public RatRecord()
        {
            ID("e_Rat");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Rat_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
