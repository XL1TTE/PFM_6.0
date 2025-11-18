
using System.Linq;

namespace UI.Elements
{
    public sealed class HorizontalLayoutElementPool : ISingletonPool<HorizontalLayoutElement>
    {
        public override void FreeElement(HorizontalLayoutElement element)
        {
            base.FreeElement(element);

            foreach (var i in element.GetChildrens())
            {
                if (i is IPoolElement poolElmt)
                {
                    poolElmt.Free();
                }
            }
        }
    }

}
