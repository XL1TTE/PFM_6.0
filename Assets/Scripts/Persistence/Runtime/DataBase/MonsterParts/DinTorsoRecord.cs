
using Domain.Components;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public class DinTorsoRecord: MonsterPartRecord{
        public DinTorsoRecord(){
            With<ID>(new ID{id = "mp_DinTorso"});
            With<BodySpritePath>(new BodySpritePath{
                    path = "Monsters/Sprites/test/Spr_Bodypart_Torso_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterTorso>();
            With<PartsOffsets>(new PartsOffsets{
               NearLegOffset = new Vector2(-0.08f, -0.11f),
               FarLegOffset = new Vector2(0.04f, -0.11f),
               NearArmOffset = new Vector2(0.02f, 0.03f),
               FarArmOffset = new Vector2(0.2f, 0.03f),
               HeadOffset = new Vector2(0.15f, 0.14f),
               BodyOffset = new Vector2(0f, 0.20f)
            });
        }
    }
}

