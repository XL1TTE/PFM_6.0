using System.Numerics;
using Core.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB{
    public class DammyLegRecord: MonsterPartRecord{
        public DammyLegRecord(){
            With<ID>(new ID { id = "mp_DammyLeg" });
            With<LegSpritePath>(new LegSpritePath{
                    FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test",
                    NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test"});
            With<TagMonsterPart>();
            With<TagMonsterLeg>();
            With<MovementData>(new MovementData{
                Movements = new Vector2Int[4]{new Vector2Int(1, 0), new Vector2Int(2, 0), 
                new Vector2Int(0,1), new Vector2Int(0, -1)}
            });
        }
    }
}

