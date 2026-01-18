using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class BeeArmRecord : BodyPartRecord
    {
        public BeeArmRecord()
        {
            ID("bp_bee-arm");

            With<Name>(new Name("BeeArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_BEE,
                m_NearSprite = GR.SPR_BP_NARM_BEE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_BEE
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_bee-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_bee-arm"
            });
        }
    }
}
