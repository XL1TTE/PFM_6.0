
using Domain.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class CowLegRecord : MonsterPartRecord
    {
        public CowLegRecord()
        {
            ID("bp_cow-leg");

            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterLeg>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cow-leg"
            });
        }
    }
}

