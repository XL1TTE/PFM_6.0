using Scellecs.Morpeh;

namespace Domain.Stats.Components
{
    public interface IStatModifierComponent : IComponent
    {
        int m_Flat { get; set; }
        float m_Multiplier { get; set; }
    }
}


