using Domain.Extentions;
using UnityEngine;

namespace Domain.Monster.Mono
{
    public class MonsterDammy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer NearLegAnchor;
        [SerializeField] private SpriteRenderer FarLegAnchor;
        [SerializeField] private SpriteRenderer BodyAnchor;
        [SerializeField] private SpriteRenderer HeadAnchor;
        [SerializeField] private SpriteRenderer NearArmAnchor;
        [SerializeField] private SpriteRenderer FarArmAnchor;

        #region Part attachment

        public void AttachNearLeg(Sprite sprite, Vector2 offset)
        {
            NearLegAnchor.sprite = sprite;
            NearLegAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        }
        public void AttachFarLeg(Sprite sprite, Vector2 offset)
        {
            FarLegAnchor.sprite = sprite;
            FarLegAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);

        }
        public void AttachNearArm(Sprite sprite, Vector2 offset)
        {
            NearArmAnchor.sprite = sprite;
            NearArmAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        }
        public void AttachFarArm(Sprite sprite, Vector2 offset)
        {
            FarArmAnchor.sprite = sprite;
            FarArmAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);

        }
        public void AttachBody(Sprite sprite, Vector2 offset)
        {
            BodyAnchor.sprite = sprite;
            BodyAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        }
        public void AttachHead(Sprite sprite, Vector2 offset)
        {
            HeadAnchor.sprite = sprite;
            HeadAnchor.transform.localPosition = new Vector3(offset.x, offset.y, 0);
        }

        #endregion


        public void ChangeColor(string hex)
        {
            if (NearLegAnchor != null)
            {
                NearLegAnchor.color = hex.ToColor();
            }
            if (FarLegAnchor != null)
            {
                FarLegAnchor.color = hex.ToColor();
            }
            if (HeadAnchor != null)
            {
                HeadAnchor.color = hex.ToColor();
            }
            if (BodyAnchor != null)
            {
                BodyAnchor.color = hex.ToColor();
            }
            if (NearArmAnchor != null)
            {
                NearArmAnchor.color = hex.ToColor();
            }
            if (FarArmAnchor != null)
            {
                FarArmAnchor.color = hex.ToColor();
            }
        }


        public void DestroySelf()
        {
            Destroy(gameObject);
        }

    }
}

