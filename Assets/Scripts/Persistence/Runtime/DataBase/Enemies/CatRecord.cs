using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class CatRecord : IDbRecord
    {
        public CatRecord()
        {
            ID("e_Cat");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Cat_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
