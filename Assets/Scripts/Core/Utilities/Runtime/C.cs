using Domain.Abilities;
using Domain.Extentions;
using UnityEngine;

namespace Core.Utilities
{
    public static class C
    {
        public static Color COLOR_PHYSICAL = Color.white;
        public static Color COLOR_BLEED = "#e21b1b".ToColor();
        public static Color COLOR_POISON = "#2f7c1d".ToColor();
        public static Color COLOR_BURNING = "#e48237".ToColor();
        public static Color COLOR_HEALING = "#52da0e".ToColor();


        public static Color COLOR_NOTIFICATION_BG_DEFAULT = new Color(0, 0, 0, 0.8f);
        public static Color COLOR_WIN_NOTIFICATION = "#164200".ToColor();
        public static Color COLOR_LOST_NOTIFICATION = "#4f1313".ToColor();

        public static Color COLOR_WRONG_CELL_CHOICE_NOTIFICATION = Color.white;

        public static Color COLOR_DEFAULT = "#ffffff".ToColor();

        public static Color ToColor(this DamageType a_type)
        {
            switch (a_type)
            {
                case DamageType.BLEED_DAMAGE:
                    return COLOR_BLEED;
                case DamageType.FIRE_DAMAGE:
                    return COLOR_BURNING;
                case DamageType.POISON_DAMAGE:
                    return COLOR_POISON;
                case DamageType.PHYSICAL_DAMAGE:
                    return COLOR_PHYSICAL;
            }
            return Color.white;
        }
    }

}


