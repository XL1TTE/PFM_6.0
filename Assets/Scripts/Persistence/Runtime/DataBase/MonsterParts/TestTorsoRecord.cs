using Core.Components;
using Persistence.Components;

namespace Persistence.DB{
    public class DammyTorsoRecord: MonsterPartRecord{
        public DammyTorsoRecord(){
            With<ID>(new ID{id = "mp_DammyTorso"});
            With<BodySpritePath>(new BodySpritePath{
                    path = "Monsters/Sprites/test/Spr_Bodypart_Torso_Test"});
            With<TagMonsterPart>();
            With<TagMonsterTorso>();
        }
    }
}

