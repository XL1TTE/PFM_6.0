using System.Linq;
using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class RaccoonTorsoRecord : BodyPartRecord
    {
        public RaccoonTorsoRecord()
        {
            ID("bp_raccoon-torso");

            With<Name>(new Name("RaccoonTorsoRecord_name"));

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_RACCOON
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_RACCOON
            });
            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(-4f, -5f),
                FarLegOffset = new Vector2(15f, -5f),
                NearArmOffset = new Vector2(-4f, 4f),
                FarArmOffset = new Vector2(19f, 4f),
                HeadOffset = new Vector2(8f, 8f),
                BodyOffset = new Vector2(-3f, 11f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raccoon-torso", 1).ToArray()
            });
        }
    }
}
