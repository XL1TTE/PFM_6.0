using System.Linq;
using Domain.Components;
using Domain.Stats.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class DinHeadRecord : BodyPartRecord
    {
        public DinHeadRecord()
        {
            ID("mp_DinHead");

            With<ID>(new ID { m_Value = "mp_DinHead" });
            With<HeadSpritePath>(new HeadSpritePath
            {
                path = "Monsters/Sprites/test/Spr_Bodypart_Head_Test_1"
            }); 
            With<IconUI>(new IconUI
            {
                m_Value = Resources.Load<Sprite>("Monsters/Sprites/test/Spr_Bodypart_Head_Test_1")
            });
            With<TagBodyPart>();
            With<TagHead>();
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_DinHead", 1).ToArray()
            });
            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_din_head"
            });
        }
    }
}

