using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class CowArmRecord : BodyPartRecord
    {
        public CowArmRecord()
        {
            ID("bp_cow-arm");

            With<Name>(new Name { m_Value = "Cow's Arm" });

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_COW,
                m_NearSprite = GR.SPR_BP_NARM_COW
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_COW
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_CowArm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cow-arm"
            });
        }
    }
}
