using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class RavenLegRecord : BodyPartRecord
    {
        public RavenLegRecord()
        {
            ID("bp_raven-leg");

            With<Name>(new Name("RavenLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_RAVEN,
                m_NearSprite = GR.SPR_BP_NLEG_RAVEN
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_RAVEN
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raven-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_raven-leg"
            });
        }
    }
}

