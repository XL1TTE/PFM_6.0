
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class CowLegRecord : BodyPartRecord
    {
        public CowLegRecord()
        {
            ID("bp_cow-leg");

            With<Name>(new Name { m_Value = "Cow's Leg" });

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_COW,
                m_NearSprite = GR.SPR_BP_NARM_COW
            });
            With<TagBodyPart>();
            With<TagLeg>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cow-leg"
            });
        }
    }
}

