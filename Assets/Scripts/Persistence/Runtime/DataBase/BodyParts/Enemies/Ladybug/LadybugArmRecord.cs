using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class LadybugArmRecord : BodyPartRecord
    {
        public LadybugArmRecord()
        {
            ID("bp_ladybug-arm");

            With<Name>(new Name("LadybugArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_LADYBUG,
                m_NearSprite = GR.SPR_BP_NARM_LADYBUG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_LADYBUG
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_ladybug-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_ladybug-arm"
            });
        }
    }
}
