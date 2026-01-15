using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class DogHeadRecord : BodyPartRecord
    {
        public DogHeadRecord()
        {
            ID("bp_dog-head");

            With<Name>(new Name("DogHeadRecord_name"));

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

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_dog-head"
            });
        }
    }
}

