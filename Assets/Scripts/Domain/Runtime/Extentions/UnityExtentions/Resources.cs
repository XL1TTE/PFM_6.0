using Domain.UI.Widgets;
using UnityEngine;

namespace Domain.Extentions
{
    public static partial class UnityExtantions
    {
        public static T LoadResource<T>(this string path) where T : UnityEngine.Object
            => Resources.Load<T>(path);
    }


    public static partial class GameResources
    {

        public readonly static HealthBarView p_MonsterHealthBar = "UI/MonsterHealthBar".LoadResource<HealthBarView>();
        public readonly static HealthBarView p_EnemyHealthBar = "UI/EnemyHealthBar".LoadResource<HealthBarView>();

    }
}
