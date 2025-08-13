using Core.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DammyArmRecord: MonsterPartRecord{
        public DammyArmRecord(){
            With<ID>(new ID { id = "mp_DammyArm" });
            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
        }
    }
}

