
using Core.Bootstrap;
using Persistence.DB;

namespace Persistence.Mono{
    public class DataBaseInitInstruction : BootstrapInstruction
    {
        public override void Execute()
        {
            DataBase.Initialize();
        }
    }
}


