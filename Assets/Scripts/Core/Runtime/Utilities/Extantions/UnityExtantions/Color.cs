using UnityEngine;

namespace Core.Utilities.Extantions
{
    public static partial class UnityExtantions{
     
        public static Color ToColor(this string color)
        {
            color = color.Trim().Replace("#", "").Replace("0x", "");

            byte r = 255;
            byte g = 255;
            byte b = 255;
            byte a = 255;

            switch (color.Length)
            {
                case 3: // RGB (короткий формат)
                    r = byte.Parse(color.Substring(0, 1) + color.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(color.Substring(1, 1) + color.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(color.Substring(2, 1) + color.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    break;

                case 4: // RGBA (короткий формат)
                    r = byte.Parse(color.Substring(0, 1) + color.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(color.Substring(1, 1) + color.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(color.Substring(2, 1) + color.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    a = byte.Parse(color.Substring(3, 1) + color.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                    break;

                case 6: // RGB (полный формат)
                    r = byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    break;

                case 8: // RGBA (полный формат)
                    r = byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    a = byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    break;

                default:
                    Debug.LogWarning($"Неверный формат цвета: #{color}");
                    break;
            }

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
