using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class BeeRecord : IDbRecord
    {
        public BeeRecord()
        {
            ID("e_Bee");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Bee_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
