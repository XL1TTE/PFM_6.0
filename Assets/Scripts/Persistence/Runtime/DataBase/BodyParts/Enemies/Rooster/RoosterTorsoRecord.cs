using System.Linq;
using Domain.Extentions;
using Domain.Monster.Components;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public class RoosterTorsoRecord : BodyPartRecord
    {
        public RoosterTorsoRecord()
        {
            ID("bp_rooster-torso");

            With<Name>(new Name("RoosterTorsoRecord_name"));

            With<BodySprite>(new BodySprite
            {
                m_Value = GR.SPR_BP_TORSO_ROOSTER
            });
            With<IconUI>(new IconUI
            {
                m_Value = GR.SPR_UI_BP_TORSO_ROOSTER
            });
            With<TagBodyPart>();
            With<TagTorso>();

            With<PartsOffsets>(new PartsOffsets
            {
                NearLegOffset = new Vector2(8f, -14f),
                FarLegOffset = new Vector2(14f, -14f),
                NearArmOffset = new Vector2(8f, 3f),
                FarArmOffset = new Vector2(22f, 5f),
                HeadOffset = new Vector2(14f, 10f),
                BodyOffset = new Vector2(-7f, 16f)
            });
            With<EffectsProvider>(new EffectsProvider
            {
                m_Effects = Enumerable.Repeat("effect_rooster-torso", 1).ToArray()
            });
        }
    }
}
