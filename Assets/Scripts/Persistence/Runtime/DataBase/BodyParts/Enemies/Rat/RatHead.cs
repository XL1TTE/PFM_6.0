using System.Linq;
using Persistence.Components;

namespace Persistence.DB
{
    public class RatHeadRecord : BodyPartRecord
    {
        public RatHeadRecord()
        {
            ID("bp_rat-head");

            With<HeadSprite>(new HeadSprite
            {
                m_Value = null
            });

            With<TagBodyPart>();
            With<TagHead>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_RatHead", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_rat-head"
            });
        }
    }
}

