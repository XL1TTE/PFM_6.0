
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
               NearLegOffset = new Vector2(-7f, -13f),
               FarLegOffset = new Vector2(4, -11f),
               NearArmOffset = new Vector2(0f, 1f),
               FarArmOffset = new Vector2(20f, 0f),
               HeadOffset = new Vector2(15f, 12f),
               BodyOffset = new Vector2(0f, 21f)
            });
        }
    }
}

