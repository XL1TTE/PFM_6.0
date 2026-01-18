using Domain.Stats.Components;
using ExcelDataReader.Log;

namespace Persistence.DB
{
    public sealed class CowArmEffect : IDbRecord
    {
        public CowArmEffect()
        {
            ID("effect_cow-arm");

            With<MaxHealthModifier>(new MaxHealthModifier
            {
                m_Flat = 5
            });
            With<SpeedModifier>(new SpeedModifier
            {
                m_Flat = 1
            });
        }
    }
}
