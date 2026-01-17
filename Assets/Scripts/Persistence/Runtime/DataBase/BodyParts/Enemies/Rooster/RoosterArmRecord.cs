using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class RoosterArmRecord : BodyPartRecord
    {
        public RoosterArmRecord()
        {
            ID("bp_rooster-arm");

            With<Name>(new Name("RoosterArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_ROOSTER,
                m_NearSprite = GR.SPR_BP_NARM_ROOSTER
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_ROOSTER
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_rooster-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rooster-arm"
            });
        }
    }
}
