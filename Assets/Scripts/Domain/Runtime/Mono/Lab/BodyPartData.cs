using Domain.Stats.Components;
using UnityEngine;

public class BodyPartData
{
    public BODYPART_TYPE type;
    public string db_id;

    public Sprite icon;
    public string partName;
    public string description;

    public int hp_amount;
    public int speed_amount;

    public IResistanceModiffier.Stage res_fire;
    public IResistanceModiffier.Stage res_poison;
    public IResistanceModiffier.Stage res_bleed;

    public string ability_name;
    public string ability_desc;
    public Sprite ability_icon;
    public Sprite ability_shifts;
}


public enum BODYPART_TYPE
{
    HEAD,
    TORSO,
    ARM,
    LEG
}