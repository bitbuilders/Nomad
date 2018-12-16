using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorHueSelector : MonoBehaviour
{
    [SerializeField] ColorPicker m_colorPicker = null;

    Vector2 m_position;
    Material m_material;

    private void Start()
    {
        m_material = Instantiate(GetComponent<Image>().material);
        GetComponent<Image>().material = m_material;
    }

    public void UpdateColor(Color topRightColor)
    {
        m_material.SetColor("_ColorTopRight", topRightColor);
        SelectHue(m_position);
    }

    public void SelectHue(Vector2 point)
    {
        Color c = new Color();
        m_colorPicker.OnColorHueChange(c);
    }
}
