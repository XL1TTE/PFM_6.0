using Scellecs.Morpeh;

namespace ECS.Requests
{
    public struct DropEntityRequest : IRequestData{
        public Entity Subject;
        public Entity Target;
    }
}
