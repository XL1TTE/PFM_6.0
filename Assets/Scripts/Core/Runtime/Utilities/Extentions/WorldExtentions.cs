using Scellecs.Morpeh;

namespace Core.Utilities.Extentions{
    public static class WorldExtentions{
        
        public static bool TryGetComponent<T>(this World world, Entity entity, out T component) where T: struct, IComponent{
            var stash = world.GetStash<T>();
            if(stash.Has(entity) == false){component = default; return false;}
            component = stash.Get(entity);
            return true;
        }
    }
    
}
