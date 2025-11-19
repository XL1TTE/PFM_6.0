using UnityEngine;

public class BodyPartData
{
    public Sprite icon;
    public BODYPART_TYPE type;
    public string db_id;
}


public enum BODYPART_TYPE
{
    HEAD,
    TORSO,
    ARM,
    LEG
}