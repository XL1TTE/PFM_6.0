namespace UI.Elements
{
    public interface IUIElementPool<T> where T : IUIElement
    {
        T WarmupElement();
        void FreeElement(T element);
    }

}
