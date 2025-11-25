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

            With<ArmSprite>(new ArmSprite
            {
                m_NearSprite = GR.SPR_BP_NARM_COCKROACH,
                m_FarSprite = GR.SPR_BP_FARM_COCKROACH
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_CockroachArm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cockroach-arm"
            });
        }
    }
}
