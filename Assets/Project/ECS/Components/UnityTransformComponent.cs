using System;
using Scellecs.Morpeh;
using UnityEngine;

[Serializable]
public struct UnityTransformComponent : IComponent
{
    public Transform transform;
}

public struct MovementConfigComponent : IComponent
{
    public float speed;
}
