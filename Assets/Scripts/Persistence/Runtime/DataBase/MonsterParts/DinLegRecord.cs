
using Domain.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public class DinLegRecord: MonsterPartRecord{
        public DinLegRecord(){
            With<ID>(new ID { Value = "mp_DinLeg" });
            With<LegSpritePath>(new LegSpritePath{
                    FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test_1",
                    NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Closer_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterLeg>();
            With<MovementData>(new MovementData{
                Movements = new Vector2Int[3]{new Vector2Int(1, 1), 
                new Vector2Int(-1, -1), new Vector2Int(1, 0)}
            });
        }
    }
}

