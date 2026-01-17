using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class LadybugRecord : IDbRecord
    {
        public LadybugRecord()
        {
            ID("e_Ladybug");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Ladybug_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
