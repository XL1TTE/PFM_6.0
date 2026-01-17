using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class BearLegRecord : BodyPartRecord
    {
        public BearLegRecord()
        {
            ID("bp_bear-leg");

            With<Name>(new Name("BearLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_BEAR,
                m_NearSprite = GR.SPR_BP_NLEG_BEAR
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_BEAR
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_bear-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_bear-leg"
            });
        }
    }
}

