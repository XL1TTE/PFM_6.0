using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class RaccoonLegRecord : BodyPartRecord
    {
        public RaccoonLegRecord()
        {
            ID("bp_raccoon-leg");

            With<Name>(new Name("RaccoonLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_RACCOON,
                m_NearSprite = GR.SPR_BP_NLEG_RACCOON
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_RACCOON
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raccoon-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_raccoon-leg"
            });
        }
    }
}

