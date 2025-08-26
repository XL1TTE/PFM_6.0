
using Domain.Components;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public class DammyTorsoRecord: MonsterPartRecord{
        public DammyTorsoRecord(){
            With<ID>(new ID{id = "mp_DammyTorso"});
            With<BodySpritePath>(new BodySpritePath{
                    path = "Monsters/Sprites/test/Spr_Bodypart_Torso_Test"});
            With<TagMonsterPart>();
            With<TagMonsterTorso>();
            With<PartsOffsets>(new PartsOffsets{
               NearLegOffset = new Vector2(-0.06f, -0.11f),
               FarLegOffset = new Vector2(0.07f, -0.11f),
               NearArmOffset = new Vector2(-0.06f, 0.08f),
               FarArmOffset = new Vector2(-0.025f, 0.09f),
               HeadOffset = new Vector2(0.04f, 0.15f),
               BodyOffset = new Vector2(0f, 0.20f)
            });
        }
    }
}

