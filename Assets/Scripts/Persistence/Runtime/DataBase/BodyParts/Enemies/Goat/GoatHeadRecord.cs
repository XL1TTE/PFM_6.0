using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class GoatHeadRecord : BodyPartRecord
    {
        public GoatHeadRecord()
        {
            ID("bp_goat-head");

            With<Name>(new Name("GoatHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_GOAT
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_GOAT
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_goat-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_goat-head"
            });
        }
    }
}

