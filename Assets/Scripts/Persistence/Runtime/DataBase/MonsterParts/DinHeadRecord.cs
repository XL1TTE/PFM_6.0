using Domain.Components;
using Domain.Monster.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DinHeadRecord: MonsterPartRecord{
        public DinHeadRecord(){
            With<ID>(new ID { id = "mp_DinHead" });
            With<HeadSpritePath>(new HeadSpritePath{ 
                path = "Monsters/Sprites/test/Spr_Bodypart_Head_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterHead>();
            With<Speed>(new Speed{Value = 2.0f});
        }
    }
}

