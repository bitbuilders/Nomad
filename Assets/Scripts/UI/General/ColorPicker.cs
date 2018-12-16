using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] ColorHueSelector m_hueSelector = null;

    public delegate void OnColorChange(Color color);
    public OnColorChange OnValueChange;

    public Color Color { get; private set; }

    public void Initialize(Color startingColor)
    {
        Color = startingColor;
    }

    public void OnColorValueChange(Color color)
    {
        m_hueSelector.UpdateColor(Color);
    }

    public void OnColorHueChange(Color color)
    {
        Color = color;
        OnValueChange.Invoke(Color);
    }
}
