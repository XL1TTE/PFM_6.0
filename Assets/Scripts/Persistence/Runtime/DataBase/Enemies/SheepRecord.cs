using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class SheepRecord : IDbRecord
    {
        public SheepRecord()
        {
            ID("e_Sheep");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Sheep_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
