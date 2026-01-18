using Domain.Extentions;
using Domain.TurnSystem.Components;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class GooseHeadRecord : BodyPartRecord
    {
        public GooseHeadRecord()
        {
            ID("bp_goose-head");

            With<Name>(new Name("GooseHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_GOOSE
            });
            With<AvatarUI>(new AvatarUI
            {
                m_Value = GR.SPR_UI_AVATAR_GOOSE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_GOOSE
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_goose-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_goose-head"
            });
        }
    }
}

