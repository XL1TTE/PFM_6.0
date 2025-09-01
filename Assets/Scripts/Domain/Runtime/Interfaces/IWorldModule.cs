using Scellecs.Morpeh;


namespace Domain.ECS{
    
    public interface IWorldModule{
        int Priority {get;}
        void Initialize(World world);
    }
}


