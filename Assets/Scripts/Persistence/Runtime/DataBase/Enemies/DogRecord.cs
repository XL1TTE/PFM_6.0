using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DogRecord : IDbRecord
    {
        public DogRecord()
        {
            ID("e_Dog");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Dog_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
