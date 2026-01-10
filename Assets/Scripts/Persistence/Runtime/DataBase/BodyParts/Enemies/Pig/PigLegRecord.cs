using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class PigLegRecord : BodyPartRecord
    {
        public PigLegRecord()
        {
            ID("bp_pig-leg");

            With<Name>(new Name("PigLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_PIG,
                m_NearSprite = GR.SPR_BP_NLEG_PIG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_PIG
            });

            With<TagBodyPart>();
            With<TagLeg>();


            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_pig-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_pig-leg"
            });
        }
    }
}
