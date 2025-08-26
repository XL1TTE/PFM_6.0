using Domain.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DinArmRecord: MonsterPartRecord{
        public DinArmRecord(){
            With<ID>(new ID { id = "mp_DinArm" });
            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
        }
    }
}

