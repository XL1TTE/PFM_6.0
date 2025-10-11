namespace Domain.GameEffects
{
    [System.Serializable]
    public struct StatusEffect
    {
        public string EffectId;
        public short DurationInTurns;
        public short TurnsLeft;
    }
}
