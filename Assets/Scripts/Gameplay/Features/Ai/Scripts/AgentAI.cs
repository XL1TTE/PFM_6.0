using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.AI
{

    public interface IAIJob
    {
        UniTask DoJob();
    }

    public interface IAIModel
    {
        IAIJob MakeDecision();
    }

    public sealed class AgentAI
    {
    }
}
