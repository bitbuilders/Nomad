using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorValueSelector : MonoBehaviour
{
    [SerializeField] ColorPicker m_colorPicker = null;

    Slider m_slider;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    public void OnValueChange()
    {
        float a = m_slider.value;
        Color c = new Color(a, a, a, 1.0f);

        m_colorPicker.OnColorValueChange(c);
    }
}
