using Scellecs.Morpeh;

namespace Project.AI
{
    public sealed class AIExecutionContext
    {
        public World m_World;
        public Entity m_Agent;

        public AIExecutionContext(Entity a_agent, World a_world)
        {
            m_Agent = a_agent;
            m_World = a_world;
        }
    }
}
