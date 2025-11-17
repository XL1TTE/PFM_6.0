
using Domain.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class DinLegRecord : BodyPartRecord
    {
        public DinLegRecord()
        {

            ID("mp_DinLeg");


            With<ID>(new ID { m_Value = "mp_DinLeg" });
            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagBodyPart>();
            With<TagLeg>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_leg"
            });
        }
    }
}

