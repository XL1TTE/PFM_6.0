using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using System.Linq;

namespace Persistence.DB
{
    public class CatLegRecord : BodyPartRecord
    {
        public CatLegRecord()
        {
            ID("bp_cat-leg");

            With<Name>(new Name("CatLegRecord_name"));

            With<LegSprite>(new LegSprite
            {
                m_FarSprite = GR.SPR_BP_FLEG_CAT,
                m_NearSprite = GR.SPR_BP_NLEG_CAT
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_LEG_CAT
            });
            With<TagBodyPart>();
            With<TagLeg>();

            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_cat-leg", 1).ToArray()
            });

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cat-leg"
            });
        }
    }
}

