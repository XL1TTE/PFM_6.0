using Cysharp.Threading.Tasks;

namespace Project.AI
{
    public interface IAIJob
    {
        UniTask DoJob(AIExecutionContext context);
    }
}
