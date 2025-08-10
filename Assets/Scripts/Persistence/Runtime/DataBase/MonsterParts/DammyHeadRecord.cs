using Core.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DammyHeadRecord: MonsterPartRecord{
        public DammyHeadRecord(){
            With<ID>(new ID { id = "mp_DammyHead" });
            With<HeadSpritePath>(new HeadSpritePath{ 
                path = "Monsters/Sprites/test/Spr_Bodypart_Head_Test"});
            With<TagMonsterPart>();
            With<TagMonsterHead>();
        }
    }
}

