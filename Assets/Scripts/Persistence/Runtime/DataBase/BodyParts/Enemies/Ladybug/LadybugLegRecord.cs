using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class LadybugLegRecord : BodyPartRecord
    {
        public LadybugLegRecord()
        {
            ID("bp_ladybug-leg");

            With<Name>(new Name("LadybugLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_LADYBUG,
                m_NearSprite = GR.SPR_BP_NLEG_LADYBUG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_LADYBUG
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_ladybug-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_ladybug-leg"
            });
        }
    }
}

