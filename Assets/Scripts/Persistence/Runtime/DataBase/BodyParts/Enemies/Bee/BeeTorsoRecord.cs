using System.Linq;
using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class BeeTorsoRecord : BodyPartRecord
    {
        public BeeTorsoRecord()
        {
            ID("bp_bee-torso");

            With<Name>(new Name("BeeTorsoRecord_name"));

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_BEE
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_BEE
            });
            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(-12f, -8f),
                FarLegOffset = new Vector2(0f, -4f),
                NearArmOffset = new Vector2(0f, 9f),
                FarArmOffset = new Vector2(10f, 11f),
                HeadOffset = new Vector2(5f, 15f),
                BodyOffset = new Vector2(3f, 14f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_bee-torso", 1).ToArray()
            });
        }
    }
}
