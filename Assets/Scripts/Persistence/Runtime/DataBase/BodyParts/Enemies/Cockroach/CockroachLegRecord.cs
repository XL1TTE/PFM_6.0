
using Domain.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class CockroachLegRecord : BodyPartRecord
    {
        public CockroachLegRecord()
        {
            ID("bp_cockroach-leg");

            With<Name>(new Name { m_Value = "Cockroach's Leg" });

            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagBodyPart>();
            With<TagLeg>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cockroach-leg"
            });
        }
    }
}

