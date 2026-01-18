using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class RaccoonRecord : IDbRecord
    {
        public RaccoonRecord()
        {
            ID("e_Raccoon");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Raccoon_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
