using Persistence.Components;

namespace Persistence.DB
{
    public class RatHeadRecord : BodyPartRecord
    {
        public RatHeadRecord()
        {
            ID("bp_rat-head");

            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagBodyPart>();
            With<TagHead>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rat-head"
            });
        }
    }
}

