using Scellecs.Morpeh;

namespace Extantions{
    
    public static class EntityExntations{
        
        public static bool IsExist(this Entity entity){
            if(entity.Id == 0){return false;}
            return true;
        }
    }
    
}
