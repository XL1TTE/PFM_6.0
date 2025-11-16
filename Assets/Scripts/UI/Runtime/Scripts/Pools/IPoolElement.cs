using System;

namespace UI.Elements
{
    public interface IPoolElement
    {

        event Action OnFree;
        void Free();
    }

}
