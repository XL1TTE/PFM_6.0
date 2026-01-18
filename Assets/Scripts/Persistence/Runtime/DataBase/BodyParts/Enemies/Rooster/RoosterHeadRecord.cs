using Domain.Extentions;
using Domain.TurnSystem.Components;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class RoosterHeadRecord : BodyPartRecord
    {
        public RoosterHeadRecord()
        {
            ID("bp_rooster-head");

            With<Name>(new Name("RoosterHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_ROOSTER
            });
            With<AvatarUI>(new AvatarUI
            {
                m_Value = GR.SPR_UI_AVATAR_ROOSTER
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_ROOSTER
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_rooster-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rooster-head"
            });
        }
    }
}

