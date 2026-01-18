using Domain.Extentions;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class GooseRecord : IDbRecord
    {
        public GooseRecord()
        {
            ID("e_Goose");

            With<PrefabComponent>(new PrefabComponent
            {
                Value = "Enemies/p_Goose_Enemy".LoadResource<GameObject>()
            });
        }
    }

}
