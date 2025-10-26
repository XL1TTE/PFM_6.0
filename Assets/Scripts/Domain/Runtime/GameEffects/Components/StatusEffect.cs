namespace Domain.GameEffects
{
    [System.Serializable]
    public struct StatusEffect
    {
        public string m_EffectId;
        public int m_DurationInTurns;
        public int m_TurnsLeft;
    }
}
