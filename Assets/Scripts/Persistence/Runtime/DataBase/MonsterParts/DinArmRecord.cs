using Domain.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DinArmRecord: MonsterPartRecord{
        public DinArmRecord(){
            With<ID>(new ID { Value = "mp_DinArm" });
            With<ArmSpritePath>(new ArmSpritePath
            {
                FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Further_Test_1",
                NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Arm_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterArm>();
            With<AttackData>(new AttackData{Attacks = new UnityEngine.Vector2Int[2]{
                new UnityEngine.Vector2Int(1,0), new UnityEngine.Vector2Int(1,1)
            }});
        }
    }
}

