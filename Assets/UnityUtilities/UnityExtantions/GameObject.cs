using UnityEngine;

namespace UnityUtilities
{
    public static partial class UnityExtantions
    {
        public static bool TryFindComponent<T>(this GameObject obj, out T unknown){
            
            // Check in self
            if(obj.TryGetComponent<T>(out unknown)){return true;}
            
            // Check in childrens
            for(int i = 0; i < obj.transform.childCount; i++){
                if(obj.transform.GetChild(i).TryGetComponent<T>(out unknown)){return true;}
            }
            
            // Check in parent
            if(obj.GetComponentInParent<T>() is T t){unknown = t; return true;}
            
            // Else
            return false;         
        }
    }
}
