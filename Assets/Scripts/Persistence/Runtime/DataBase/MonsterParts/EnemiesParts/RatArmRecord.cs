using Persistence.Components;

namespace Persistence.DB
{
    public class RatArmRecord : MonsterPartRecord
    {
        public RatArmRecord()
        {
            ID("bp_rat-arm");

            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });

            With<TagMonsterPart>();
            With<TagMonsterArm>();

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rat_arm"
            });
        }
    }
}

