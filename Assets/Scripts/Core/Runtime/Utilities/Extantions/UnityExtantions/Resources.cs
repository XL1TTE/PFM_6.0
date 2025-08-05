using UnityEngine;

namespace Core.Utilities.Extantions
{
    public static partial class UnityExtantions{
        public static T LoadResource<T>(this string path) where T : UnityEngine.Object
            => Resources.Load<T>(path);
    }
}
