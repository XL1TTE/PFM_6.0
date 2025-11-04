
using Core.Bootstrap;
using Persistence.DS;

namespace Persistence.Mono
{
    public class DataStorageInitInstruction : BootstrapInstruction
    {
        public override void Execute()
        {
            DataStorage.Initialize();
        }
    }
}


