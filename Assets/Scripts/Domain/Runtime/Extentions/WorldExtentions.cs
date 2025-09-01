using Domain.ECS;
using Scellecs.Morpeh;

namespace Domain.Extentions
{
    public static class WorldExtentions{
        
        public static bool TryGetComponent<T>(this World world, Entity entity, out T component) where T: struct, IComponent{
            var stash = world.GetStash<T>();
            if(stash.Has(entity) == false){component = default; return false;}
            component = stash.Get(entity);
            return true;
        }
        
        
        public static void AddModule(this World world, IWorldModule module){
            module.Initialize(world);
        }
    }
    
}
