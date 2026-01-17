using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class HorseLegRecord : BodyPartRecord
    {
        public HorseLegRecord()
        {
            ID("bp_horse-leg");

            With<Name>(new Name("HorseLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_HORSE,
                m_NearSprite = GR.SPR_BP_NLEG_HORSE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_HORSE
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_horse-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_horse-leg"
            });
        }
    }
}

