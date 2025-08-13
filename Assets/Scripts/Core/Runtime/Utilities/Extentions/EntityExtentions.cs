using Scellecs.Morpeh;

namespace Core.Utilities.Extentions{
    
    public static class EntityExtentions{
        
        public static bool IsExist(this Entity entity){
            if(entity.Id == 0){return false;}
            return true;
        }
    }
    
}
