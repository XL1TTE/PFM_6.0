using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class RoosterRecord : IDbRecord
    {
        public RoosterRecord()
        {
            ID("e_Rooster");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Rooster_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
