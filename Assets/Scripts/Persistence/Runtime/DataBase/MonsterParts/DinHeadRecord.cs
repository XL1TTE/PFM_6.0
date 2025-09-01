using Domain.Components;
using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DinHeadRecord: MonsterPartRecord{
        public DinHeadRecord(){
            With<ID>(new ID { Value = "mp_DinHead" });
            With<HeadSpritePath>(new HeadSpritePath{ 
                path = "Monsters/Sprites/test/Spr_Bodypart_Head_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterHead>();
            With<Speed>(new Speed{Value = 2.0f});
            With<Health>(new Health { Value = 10.0f});
        }
    }
}

