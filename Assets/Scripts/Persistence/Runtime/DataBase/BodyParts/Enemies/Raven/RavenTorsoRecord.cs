using System.Linq;
using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class RavenTorsoRecord : BodyPartRecord
    {
        public RavenTorsoRecord()
        {
            ID("bp_raven-torso");

            With<Name>(new Name("RavenTorsoRecord_name"));

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_RAVEN
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_RAVEN
            });
            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(-10f, -10f),
                FarLegOffset = new Vector2(-3, -7f),
                NearArmOffset = new Vector2(2f, 7f),
                FarArmOffset = new Vector2(20f, 6f),
                HeadOffset = new Vector2(15f, 10f),
                BodyOffset = new Vector2(0f, 12f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raven-torso", 1).ToArray()
            });
        }
    }
}
