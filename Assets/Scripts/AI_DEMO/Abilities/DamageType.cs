namespace Domain.Abilities
{
    public enum DamageType : byte
    {
        PHYSICAL_DAMAGE,
        FIRE_DAMAGE,
        BLEED_DAMAGE,
        POISON_DAMAGE
    }

    public enum OnDamageTags : byte
    {
        BLOCKED,
        IMMUNED,
        RESISTANT,
        WEAKNESS
    }

}
