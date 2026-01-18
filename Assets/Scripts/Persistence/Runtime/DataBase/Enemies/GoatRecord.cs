using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class GoatRecord : IDbRecord
    {
        public GoatRecord()
        {
            ID("e_Goat");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Goat_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
