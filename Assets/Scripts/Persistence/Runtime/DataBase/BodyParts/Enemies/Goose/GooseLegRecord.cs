using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class GooseLegRecord : BodyPartRecord
    {
        public GooseLegRecord()
        {
            ID("bp_goose-leg");

            With<Name>(new Name("GooseLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_GOOSE,
                m_NearSprite = GR.SPR_BP_NLEG_GOOSE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_GOOSE
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_goose-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_goose-leg"
            });
        }
    }
}

