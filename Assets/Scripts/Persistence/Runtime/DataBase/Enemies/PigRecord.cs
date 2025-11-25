using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class PigRecord : IDbRecord
    {
        public PigRecord()
        {
            ID("e_Pig");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Pig_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
