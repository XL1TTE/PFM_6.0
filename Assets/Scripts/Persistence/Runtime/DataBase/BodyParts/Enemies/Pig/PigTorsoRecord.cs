using System.Linq;
using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class PigTorsoRecord : BodyPartRecord
    {
        public PigTorsoRecord()
        {
            ID("bp_pig-torso");

            With<Name>(new Name("PigTorsoRecord_name"));

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_PIG
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_PIG
            });
            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(-12f, -13f),
                FarLegOffset = new Vector2(8, -11f),
                NearArmOffset = new Vector2(-15f, 2f),
                FarArmOffset = new Vector2(11f, 4f),
                HeadOffset = new Vector2(0f, 10f),
                BodyOffset = new Vector2(0f, 21f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_pig-torso", 1).ToArray()
            });
        }
    }
}
