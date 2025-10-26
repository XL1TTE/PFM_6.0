using System.Linq;
using Domain.Components;
using Domain.Stats.Components;
using Persistence.Components;

namespace Persistence.DB
{
    public class DinHeadRecord : MonsterPartRecord
    {
        public DinHeadRecord()
        {
            ID("mp_DinHead");

            With<ID>(new ID { Value = "mp_DinHead" });
            With<HeadSpritePath>(new HeadSpritePath
            {
                path = "Monsters/Sprites/test/Spr_Bodypart_Head_Test_1"
            });
            With<TagMonsterPart>();
            With<TagMonsterHead>();
            With<Effects>(new Effects
            {
                m_Effects = Enumerable.Repeat("effect_DinHead", 500).ToArray()
            });
        }
    }
}

