using System.Linq;
using Domain.Extentions;
using Persistence.Components;

namespace Persistence.DB
{
    public class PigHeadRecord : BodyPartRecord
    {
        public PigHeadRecord()
        {
            ID("bp_pig-head");

            With<Name>(new Name ("PigHeadRecord_name"));

            With<HeadSprite>(new HeadSprite
            {
                m_Value = GR.SPR_BP_HEAD_PIG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_HEAD_PIG
            });

            With<TagBodyPart>();
            With<TagHead>();


            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_pig-head", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_pig-head"
            });
        }
    }
}
