using Domain.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class DinArmRecord : MonsterPartRecord
    {
        public DinArmRecord()
        {

            ID("mp_DinArm");

            With<ID>(new ID { m_Value = "mp_DinArm" });
            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm"
            });
        }
    }
    public class Din2ArmRecord : MonsterPartRecord
    {
        public Din2ArmRecord()
        {

            ID("mp_Din2Arm");

            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm2"
            });
        }
    }
    public class Din3ArmRecord : MonsterPartRecord
    {
        public Din3ArmRecord()
        {

            ID("mp_Din3Arm");

            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_arm3"
            });
        }
    }
}

