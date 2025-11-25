using Domain.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class DinArmRecord : BodyPartRecord
    {
        public DinArmRecord()
        {

            ID("mp_DinArm");


            With<Name>(new Name("Din arm"));
            With<Description>(new Description("smells like cheese"));



            With<ID>(new ID { m_Value = "mp_DinArm" });
            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<IconUI>(new IconUI
            {
                m_Value = Resources.Load<Sprite>("Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1")
            });
            With<TagBodyPart>();
            With<TagArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm"
            });
        }
    }
    public class Din2ArmRecord : BodyPartRecord
    {
        public Din2ArmRecord()
        {

            ID("mp_Din2Arm");
            With<Name>(new Name("Din arm"));
            With<Description>(new Description("smells like cheese"));


            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<IconUI>(new IconUI
            {
                m_Value = Resources.Load<Sprite>("Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1")
            });
            With<TagBodyPart>();
            With<TagArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm2"
            });
        }
    }
    public class Din3ArmRecord : BodyPartRecord
    {
        public Din3ArmRecord()
        {

            ID("mp_Din3Arm");
            With<Name>(new Name("Din arm"));
            With<Description>(new Description("smells like cheese"));


            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagBodyPart>();
            With<TagArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm3"
            });
        }
    }
}

