
using System.Linq;
using UnityEngine;

namespace Domain.Extentions
{
    public static partial class UnityExtantions
    {
        public static Sprite LoadFromSheet(this string sheetPath, string spriteName)
            => Resources.LoadAll<Sprite>(sheetPath).FirstOrDefault(s => s.name == spriteName);
    }
}
