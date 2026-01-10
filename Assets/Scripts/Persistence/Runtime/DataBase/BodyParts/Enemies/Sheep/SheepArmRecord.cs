using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using System.Linq;
using UnityEngine;

namespace Persistence.DB
{
    public class SheepArmRecord : BodyPartRecord
    {
        public SheepArmRecord()
        {
            ID("bp_sheep-arm");

            With<Name>(new Name { m_Value = "Sheep's Arm" });

            With<ArmSprite>(new ArmSprite
            {
                m_FarSprite = GR.SPR_BP_FARM_SHEEP,
                m_NearSprite = GR.SPR_BP_NARM_SHEEP
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_ARM_SHEEP
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

            With<Name>(new Name ("SheepHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_SHEEP
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_SHEEP
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
    public class SheepTorsoRecord : BodyPartRecord
    {
        public SheepTorsoRecord()
        {
            ID("bp_sheep-torso");

            With<Name>(new Name { m_Value = "Sheep's Body" });

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_SHEEP
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_SHEEP
            });

            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(-4f, -7f),
                FarLegOffset = new Vector2(4f, -7f),
                NearArmOffset = new Vector2(-7f, 4f),
                FarArmOffset = new Vector2(7f, 4f),
                HeadOffset = new Vector2(1f, 9f),
                BodyOffset = new Vector2(0f, 12f)
            });

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_sheep-torso", 1).ToArray()
            });
        }
    }

    public class SheepLegRecord : BodyPartRecord
    {
        public SheepLegRecord()
        {

            ID("bp_sheep-leg");

            With<Name>(new Name { m_Value = "Sheep's Leg" });

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_SHEEP,
                m_NearSprite = GR.SPR_BP_NLEG_SHEEP
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_SHEEP
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
