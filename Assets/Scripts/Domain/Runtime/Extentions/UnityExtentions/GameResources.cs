using Domain.FloatingDamage;
using Domain.UI.Widgets;

namespace Domain.Extentions
{
    public static partial class GameResources
    {

        public readonly static HealthBarView p_MonsterHealthBar = "UI/MonsterHealthBar".LoadResource<HealthBarView>();
        public readonly static HealthBarView p_EnemyHealthBar = "UI/EnemyHealthBar".LoadResource<HealthBarView>();


        public readonly static FloatingDamageView p_FloatingDamage = "UI/FloatingDamage".LoadResource<FloatingDamageView>();
    }
}
