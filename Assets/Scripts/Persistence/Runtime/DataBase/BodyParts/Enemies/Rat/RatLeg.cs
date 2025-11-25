
using System.Linq;
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class RatLegRecord : BodyPartRecord
    {
        public RatLegRecord()
        {

            ID("bp_rat-leg");

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_RAT,
                m_NearSprite = GR.SPR_BP_NLEG_RAT
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

