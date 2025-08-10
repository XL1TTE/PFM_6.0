using UnityEngine;

namespace Gameplay.Features.Monster{
    public class MonsterDammy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer NearLegAnchor;
        [SerializeField] private SpriteRenderer FarLegAnchor;
        [SerializeField] private SpriteRenderer BodyAnchor;
        [SerializeField] private SpriteRenderer HeadAnchor;
        [SerializeField] private SpriteRenderer NearArmAnchor;
        [SerializeField] private SpriteRenderer FarArmAnchor;
        
        public void AttachNearLeg(Sprite sprite){
            NearLegAnchor.sprite = sprite;
        }
        public void AttachFarLeg(Sprite sprite){
            FarLegAnchor.sprite = sprite;
        }
        public void AttachNearArm(Sprite sprite){
            NearArmAnchor.sprite = sprite;
        }
        public void AttachFarArm(Sprite sprite){
            FarArmAnchor.sprite = sprite;
        }
        public void AttachBody(Sprite sprite){
            BodyAnchor.sprite = sprite;
        }
        public void AttachHead(Sprite sprite){
            HeadAnchor.sprite = sprite;
        }
        
    }
}

