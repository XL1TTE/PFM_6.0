using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class GoatLegRecord : BodyPartRecord
    {
        public GoatLegRecord()
        {
            ID("bp_goat-leg");

            With<Name>(new Name("GoatLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_GOAT,
                m_NearSprite = GR.SPR_BP_NLEG_GOAT
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_GOAT
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_goat-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_goat-leg"
            });
        }
    }
}

