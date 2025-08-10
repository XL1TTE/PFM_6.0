using Core.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DammyLegRecord: MonsterPartRecord{
        public DammyLegRecord(){
            With<ID>(new ID { id = "mp_DammyLeg" });
            With<LegSpritePath>(new LegSpritePath{
                    FarSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test",
                    NearSprite = "Monsters/Sprites/test/Spr_Bodypart_Leg_Further_Test"});
            With<TagMonsterPart>();
            With<TagMonsterLeg>();
        }
    }
}

