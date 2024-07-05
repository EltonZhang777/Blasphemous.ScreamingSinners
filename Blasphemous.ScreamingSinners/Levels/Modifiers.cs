using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using UnityEngine;

namespace Blasphemous.ScreamingSinners.Levels;

/// <summary>
/// Modifier for solid objects that cannot be passed by TPO
/// </summary>
public class SolidObjectModifier : IModifier
{
    private readonly string _name;
    private readonly Vector2 _size;
    private readonly Vector2 _offset;

    /// <summary>
    /// Specify the size of the collider (offset set to zero)
    /// </summary>
    public SolidObjectModifier(Vector2 size)
    {
        _size = size;
        _offset = Vector2.zero;
    }

    /// <summary>
    /// Specify the size and offset of the collider
    /// </summary>
    public SolidObjectModifier(Vector2 size, Vector2 offset)
    {
        _size = size;
        _offset = offset;
    }

    /// <summary>
    /// Adds a collider component and sets the layer to `Floor`
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
        obj.layer = LayerMask.NameToLayer("Floor");

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;
    }
}

/// <summary>
/// Modifier for platform objects 
/// that TPO can pass through from below, 
/// or drop from above using [JUMP] + [DOWN]
/// </summary>
public class DroppablePlatformModifier : IModifier
{
    private readonly string _name;
    private readonly Vector2 _size;
    private readonly Vector2 _offset;

    /// <summary>
    /// Specify the size of the collider (offset set to zero)
    /// </summary>
    public DroppablePlatformModifier(Vector2 size)
    {
        _size = size;
        _offset = Vector2.zero;
    }

    /// <summary>
    /// Specify the size and offset of the collider
    /// </summary>
    public DroppablePlatformModifier(Vector2 size, Vector2 offset)
    {
        _size = size;
        _offset = offset;
    }

    /// <summary>
    /// Adds a collider component and sets the layer to `OneWayDown`
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
        obj.layer = LayerMask.NameToLayer("OneWayDown");

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;
    }
}


/// <summary>
/// Modifier for ladders
/// </summary>
public class LadderModifier : IModifier
{
    private Vector2 _colliderSize;

    ///<inheritdoc/>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = "LadderTrigger";

        //obj.tag = null;
        obj.layer = LayerMask.NameToLayer("Ladder");

        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = _colliderSize;
    }

    /// <summary>
    /// Construct a basic ladder with hitbox size (0.5, 0.8)
    /// </summary>
    public LadderModifier()
    {
        _colliderSize = new Vector2(0.5f, 0.8f);
    }

    /// <summary>
    /// Construct a ladder with custom hitbox size
    /// </summary>
    public LadderModifier(Vector2 size)
    {
        _colliderSize = size;
    }
}


