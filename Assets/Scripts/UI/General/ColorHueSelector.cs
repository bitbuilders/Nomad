using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorHueSelector : MonoBehaviour
{
    [SerializeField] ColorPicker m_colorPicker = null;

    Vector3Int[] sections;
    float section;
    public Slider m_slider;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
        sections = new Vector3Int[7] { new Vector3Int(1, 0, 0),
            new Vector3Int(1, 0, 1),    new Vector3Int(0, 0, 1),
            new Vector3Int(0, 1, 1),    new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),    new Vector3Int(1, 0, 0) };
        section = 1.0f / 6.0f;
    }

    public void SetValue(float value)
    {
        m_slider.value = value;
    }

    public Color GetColor()
    {
        float a = m_slider.value;
        int lowerindex = (int)(a / section);
        Color c1 = new Color(sections[lowerindex].x, sections[lowerindex].y, sections[lowerindex].z, 255);
        Color c2 = new Color(sections[lowerindex + 1].x, sections[lowerindex + 1].y, sections[lowerindex + 1].z, 255);
        Color c = Color.Lerp(c1, c2, (a - section * lowerindex) / (section));

        return c;
    }

    public void OnValueChange()
    {
        m_colorPicker.OnColorHueChange(GetColor());
    }
}
