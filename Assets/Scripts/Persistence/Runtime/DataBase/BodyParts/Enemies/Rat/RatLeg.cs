
using System.Linq;
using Domain.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class RatLegRecord : BodyPartRecord
    {
        public RatLegRecord()
        {

            ID("bp_rat-leg");

            With<LegSpritePath>(new LegSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_RatLeg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rat-leg"
            });
        }
    }
}

