using Core.Utilities.Extentions;
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
        
        #region Part attachment
        
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
        
        #endregion
        
        
        public void ChangeColor(string hex){
            if(NearLegAnchor != null){
                NearLegAnchor.color = hex.ToColor();
            }
            if(FarLegAnchor != null){
                FarLegAnchor.color = hex.ToColor();
            }
            if(HeadAnchor != null){
                HeadAnchor.color = hex.ToColor();
            }
            if(BodyAnchor != null){
                BodyAnchor.color = hex.ToColor();
            } 
            if(NearArmAnchor != null){
                NearArmAnchor.color = hex.ToColor();
            }
            if(FarArmAnchor != null){
                FarArmAnchor.color = hex.ToColor();
            }
        }
    
        
        public void DestroySelf(){
            Destroy(gameObject);
        }
    
    }
}

