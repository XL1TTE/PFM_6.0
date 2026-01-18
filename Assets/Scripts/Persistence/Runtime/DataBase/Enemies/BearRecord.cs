using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class BearRecord : IDbRecord
    {
        public BearRecord()
        {
            ID("e_Bear");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Bear_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
