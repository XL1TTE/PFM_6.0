using Persistence.Components;

namespace Persistence.DB
{
    public class CowHeadRecord : MonsterPartRecord
    {
        public CowHeadRecord()
        {

            ID("bp_cow-head");

            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterHead>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cow-head"
            });
        }
    }
}

