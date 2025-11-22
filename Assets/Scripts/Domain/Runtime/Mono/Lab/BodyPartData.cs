using UnityEngine;

public class BodyPartData
{
    public Sprite icon;
    public BODYPART_TYPE type;
    public string db_id;
    public string partName;
    public string description;
}


public enum BODYPART_TYPE
{
    HEAD,
    TORSO,
    ARM,
    LEG
}