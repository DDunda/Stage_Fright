using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFadeScript : MonoBehaviour
{
    public float startColorValue = 0; // at or below this value, the object is completely its original color
    public float endColorValue = 100; // at or above this value, the object is completely the new color
    private Color startColor;
    public Color endColor = new Color(1, 1, 1, 0);
    private SpriteRenderer sprite; // the sprite we're going to fade

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        startColor = sprite.color;
    }

    public void UpdateColor(float value)
    {
        value = Math.Max(startColorValue, value); // if less than startColorValue, set it to that
        value = Math.Min(endColorValue, value); // if more than endColorValue, set it to that
        var eValue = (value - startColorValue) / endColorValue; // how much of the new color to keep (0 - 1)
        var sValue = 1 - eValue; // how much of the original color to keep (0 - 1)

        // average the RGBA values between start color and end color, and create a new color with them
        var spriteColor = new Color(
            (startColor.r * sValue) + (endColor.r * eValue),
            (startColor.g * sValue) + (endColor.g * eValue),
            (startColor.b * sValue) + (endColor.b * eValue),
            (startColor.a * sValue) + (endColor.a * eValue)
            );
        sprite.color = spriteColor; // write the new color value back to the sprite (you can't write directly to sprite.color for some reason)
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
