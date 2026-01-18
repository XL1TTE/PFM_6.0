using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class RavenArmRecord : BodyPartRecord
    {
        public RavenArmRecord()
        {
            ID("bp_raven-arm");

            With<Name>(new Name("RavenArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_RAVEN,
                m_NearSprite = GR.SPR_BP_NARM_RAVEN
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_RAVEN
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raven-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_raven-arm"
            });
        }
    }
}
