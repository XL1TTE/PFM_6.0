using System.Threading.Tasks;
using Scellecs.Morpeh;

namespace Domain.Ability
{
    public struct AbilityContext
    {
        public Entity m_Caster { get; }
        public Entity m_Target { get; }
        public World m_World { get; }

        public AbilityContext(Entity a_Caster, Entity a_Target, World a_World)
        {
            this.m_Caster = a_Caster;
            this.m_Target = a_Target;
            this.m_World = a_World;
        }
    }
}
