using Scellecs.Morpeh;

namespace Domain.Extentions
{
    
    public static class EntityExtentions{
        
        public static bool IsExist(this Entity entity){
            if(entity.Id == 0){return false;}
            return true;
        }
        public static bool IsDisposed(this Entity entity, World world){
            if(world.IsDisposed(entity)){return true;}
            return false;
        }
        public static bool isNullOrDisposed(this Entity entity, World world){
            if(world.IsDisposed(entity)){return true;}
            if(entity.IsExist() == false){return true;}
            return false;
        }
    }
    
}
