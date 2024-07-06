using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using System;
using System.Collections.Generic;
using Tools.Level.Layout;
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
    private float _segmentLength;

    /// <summary>
    /// Destroys original SpriteRender, 
    /// then creates a series of SpriteRenderers for each segment of the ladder, 
    /// before creating the one actual hitbox
    /// </summary>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = "LadderTrigger";
        //obj.tag = null;
        obj.layer = LayerMask.NameToLayer("Ladder");
        if (data.properties.Length < 2)
        {
            Main.ScreamingSinners.LogError($"Ladder {data.id} top and/or bottom position not specified!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        // unzip the string of top and bottom to floats
        float top;
        float bottom;
        // top and bottom must be the first 2 arguments in `properties`
        if (data.properties[0].StartsWith("top") && data.properties[1].StartsWith("bottom"))
        {
            top = float.Parse(data.properties[0].Substring(data.properties[0].IndexOf('=') + 1));
            bottom = float.Parse(data.properties[1].Substring(data.properties[1].IndexOf('=') + 1));
        }
        else if (data.properties[1].StartsWith("top") && data.properties[0].StartsWith("bottom"))
        {
            bottom = float.Parse(data.properties[0].Substring(data.properties[0].IndexOf('=') + 1));
            top = float.Parse(data.properties[1].Substring(data.properties[1].IndexOf('=') + 1));
        }
        else
        {
            Main.ScreamingSinners.LogError($"Ladder {data.id} top and/or bottom position " +
                $"not specified in the first two property arguments!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        // calculate the integer number of ladder segments based on tile length
        // use floor rounding to fix the bottom position to integer segment length
        int numSegments = (int)Mathf.Floor((top - bottom) / _segmentLength);
        bottom = top - (numSegments * _segmentLength);
        Main.ScreamingSinners.Log($"ladder id: {data.id}, " +
            $"top: {top}, bottom: {bottom}, numSegments = {numSegments}");
        if (numSegments <= 0)
        {
            Main.ScreamingSinners.LogError($"Ladder {data.id} top and bottom position " +
                $"specified is shorter than 1 segment!\n" +
                $"Skipped registering ladder object!");
            UnityEngine.Object.Destroy(obj);
            return;
        }

        // create `GameObject`s of each segment
        // and adjust each segment's position
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        for (int i = 0; i < numSegments; i++)
        {
            GameObject newObject = UnityEngine.Object.Instantiate(
                sprite.gameObject,
                UnityEngine.Object.FindObjectOfType<LevelInitializer>().transform);
            newObject.transform.position = new Vector3(
                obj.transform.position.x,
                top - (i + 0.5f)*_segmentLength,
                0f);
            newObject.name = $"{data.id}_LadderSegment_{i}";
            newObject.layer = LayerMask.NameToLayer("Default");
        }
        UnityEngine.Object.Destroy(sprite); // destroy the original sprite

        // create one single collider for the ladder
        obj.transform.position = new Vector3(
            obj.transform.position.x, 
            (top + bottom)/2, 
            0f);
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(
            0.5f, 
            _segmentLength * numSegments);
    }


    /// <summary>
    /// Construct a ladder with custom single segment y-length
    /// </summary>
    public LadderModifier(float segmentLength)
    {
        _segmentLength = segmentLength;
    }
}


