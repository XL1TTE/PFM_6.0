using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class BeeLegRecord : BodyPartRecord
    {
        public BeeLegRecord()
        {
            ID("bp_bee-leg");

            With<Name>(new Name("BeeLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_DOG,
                m_NearSprite = GR.SPR_BP_NLEG_DOG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_DOG
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_bee-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_bee-leg"
            });
        }
    }
}

