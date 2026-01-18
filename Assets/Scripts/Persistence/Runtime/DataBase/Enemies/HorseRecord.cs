using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class HorseRecord : IDbRecord
    {
        public HorseRecord()
        {
            ID("e_Horse");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Horse_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
