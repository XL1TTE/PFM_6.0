using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class GooseArmRecord : BodyPartRecord
    {
        public GooseArmRecord()
        {
            ID("bp_goose-arm");

            With<Name>(new Name("GooseArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_COW,
                m_NearSprite = GR.SPR_BP_NARM_COW
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_COCKROACH
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_goose-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_goose-arm"
            });
        }
    }
}
