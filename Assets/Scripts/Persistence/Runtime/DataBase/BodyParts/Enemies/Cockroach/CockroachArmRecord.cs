using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class CockroachArmRecord : BodyPartRecord
    {
        public CockroachArmRecord()
        {
            ID("bp_cockroach-arm");

            With<Name>(new Name("CockroachArmRecord_name"));

            With<ArmSprite>(new ArmSprite
            {
                m_NearSprite = GR.SPR_BP_NARM_COCKROACH,
                m_FarSprite = GR.SPR_BP_FARM_COCKROACH
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_COCKROACH
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_cockroach-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cockroach-arm"
            });
        }
    }
}
