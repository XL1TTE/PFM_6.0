namespace Interactions
{
    public abstract class BaseInteraction
    {
        public virtual Priority m_Priority { get; } = Priority.NORMAL;
    }
}

