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
                NearLegOffset = new Vector2(-7f, -5f),
                FarLegOffset = new Vector2(-5, -2f),
                NearArmOffset = new Vector2(9f, 4f),
                FarArmOffset = new Vector2(13f, 1f),
                HeadOffset = new Vector2(22f, -4f),
                BodyOffset = new Vector2(0f, 22f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_raven-torso", 1).ToArray()
            });
        }
    }
}
