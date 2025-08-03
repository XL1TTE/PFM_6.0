using Scellecs.Morpeh;

namespace Infrastructure.Data{
    
    public static partial class Domain{
        public static readonly World _ecsWorld = World.Default;

        public static void DeleteEntity(Entity entity)
        {
            _ecsWorld.RemoveEntity(entity);
        }

    }
}
