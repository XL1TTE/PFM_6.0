using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DoveRecord : IDbRecord
    {
        public DoveRecord()
        {
            ID("e_Dove");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Dove_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
