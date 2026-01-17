using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class DoveArmRecord : BodyPartRecord
    {
        public DoveArmRecord()
        {
            ID("bp_dove-arm");

            With<Name>(new Name("DoveArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_DOVE,
                m_NearSprite = GR.SPR_BP_NARM_DOVE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_DOVE
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_dove-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_dove-arm"
            });
        }
    }
}
