using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class RaccoonArmRecord : BodyPartRecord
    {
        public RaccoonArmRecord()
        {
            ID("bp_raccoon-arm");

            With<Name>(new Name("RaccoonArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_RACCOON,
                m_NearSprite = GR.SPR_BP_NARM_RACCOON
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_RACCOON
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raccoon-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_raccoon-arm"
            });
        }
    }
}
