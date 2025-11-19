
using System.Collections.Generic;

namespace Persistence.DS
{
    public struct BodyPartsStorage : IDataStorageRecord
    {
        public Dictionary<string, int> parts;
    }
}
