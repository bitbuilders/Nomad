using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] public ColorValueSelector m_valueSelector = null;
    [SerializeField] public ColorHueSelector m_hueSelector = null;
    [SerializeField] bool m_randomStart = true;

    public delegate void OnColorChange(Color color);
    public OnColorChange OnValueChange;

    public Color Color { get; private set; }

    public void Initialize(Color startingColor)
    {
        if (m_randomStart && LocalPlayerData.Instance.FirstLoadIn)
        {
            float hue = Random.Range(0.0f, 1.0f);
            Vector2 point = new Vector2();
            point.x = Random.Range(0.0f, 1.0f);
            point.y = Random.Range(0.0f, 1.0f);
            m_hueSelector.SetValue(hue);
            m_valueSelector.UpdateColor(m_hueSelector.GetColor());
            m_valueSelector.SetPoint(point);
        }
        else
        {
            Color = startingColor;
            m_valueSelector.UpdateColor(Color);
        }
    }

    public void OnColorHueChange(Color color)
    {
        m_valueSelector.UpdateColor(color);
    }

    public void OnColorValueChange(Color color)
    {
        Color = color;
        OnValueChange.Invoke(Color);
    }
}
