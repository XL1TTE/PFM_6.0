using Scellecs.Morpeh;

namespace Domain.Map.Events
{

    public struct MapNodeClickedEvent : IEventData
    {
        public Entity node_entity;
    }

}
