using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class RavenRecord : IDbRecord
    {
        public RavenRecord()
        {
            ID("e_Raven");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Raven_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
