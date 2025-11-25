using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class SheepArmRecord : BodyPartRecord
    {
        public SheepArmRecord()
        {
            ID("bp_sheep-arm");

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_RAT,
                m_NearSprite = GR.SPR_BP_NARM_RAT
            });

            With<TagBodyPart>();
            With<TagArm>();


            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_sheep-arm", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_sheep-arm"
            });
        }
    }

    public class SheepHeadRecord : BodyPartRecord
    {
        public SheepHeadRecord()
        {
            ID("bp_sheep-head");

            With<HeadSprite>(new HeadSprite
            {
                m_Value = null
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_sheep-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_sheep-head"
            });
        }
    }

    public class SheepLegRecord : BodyPartRecord
    {
        public SheepLegRecord()
        {

            ID("bp_sheep-leg");

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_RAT,
                m_NearSprite = GR.SPR_BP_NLEG_RAT
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_sheep-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_sheep-leg"
            });
        }
    }

}
