using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class CowRecord : IDbRecord
    {
        public CowRecord()
        {
            ID("e_Cow");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Cow_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
