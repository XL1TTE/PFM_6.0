
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class DogLegRecord : BodyPartRecord
    {
        public DogLegRecord()
        {
            ID("bp_dog-leg");

            With<Name>(new Name("DogLegRecord_name"));

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
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_dog-leg"
            });
        }
    }
}

