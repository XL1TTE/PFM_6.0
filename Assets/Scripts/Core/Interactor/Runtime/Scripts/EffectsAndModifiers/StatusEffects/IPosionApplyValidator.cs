using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IPosionApplyValidator
    {
        UniTask<bool> TestOneCriteria(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world);
    }


}

