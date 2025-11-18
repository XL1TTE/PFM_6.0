using Persistence.Components;

namespace Persistence.DB
{
    public class CowArmRecord : BodyPartRecord
    {
        public CowArmRecord()
        {
            ID("bp_cow-arm");

            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });

            With<TagBodyPart>();
            With<TagArm>();

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cow-arm"
            });
        }
    }
}
