using System.Collections.Generic;
using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class RatArmRecord : BodyPartRecord
    {
        public RatArmRecord()
        {
            ID("bp_rat-arm");

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_RAT,
                m_NearSprite = GR.SPR_BP_NARM_RAT
            });

            With<TagBodyPart>();
            With<TagArm>();


            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_RatArm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rat-arm"
            });
        }
    }
}
