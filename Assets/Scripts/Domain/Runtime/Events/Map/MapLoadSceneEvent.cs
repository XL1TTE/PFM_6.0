using Scellecs.Morpeh;

namespace Domain.Map.Events
{

    public struct MapLoadSceneEvent : IEventData
    {
        public bool is_first_load;
    }

}
