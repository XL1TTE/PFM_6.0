using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class DoveHeadRecord : BodyPartRecord
    {
        public DoveHeadRecord()
        {
            ID("bp_dove-head");

            With<Name>(new Name("DoveHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_DOG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_DOG
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_dove-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_dove-head"
            });
        }
    }
}

