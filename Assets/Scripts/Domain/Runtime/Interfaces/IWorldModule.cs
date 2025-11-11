using Scellecs.Morpeh;


namespace Domain.Ecs
{

    public interface IWorldModule
    {
        int Priority { get; }
        void Initialize(World world);
    }
}


