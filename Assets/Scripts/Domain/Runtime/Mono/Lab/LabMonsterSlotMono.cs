using Domain.Monster.Mono;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using TMPro;
using UnityEngine;

namespace Project
{
    public class LabMonsterSlotMono : MonoBehaviour
    {
        public SpriteRenderer sprite_renderer;
        public TMP_Text name_text;
        public bool is_occupied;

        public void Initialize()
        {
            is_occupied = false;
            sprite_renderer.sprite = null;
            name_text.text = null;
        }
        public void OccupySelf(MonsterData data)
        {
            if (!is_occupied)
            {
                ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

                if (DataBase.TryFindRecordByID(data.Head_id, out var e_record))
                {
                    if (DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                    {
                        sprite_renderer.sprite = e_icon.m_Value;
                    }
                }

                name_text.text = data.NearArm_id;

                is_occupied = true;
            }
        }
    }
}
