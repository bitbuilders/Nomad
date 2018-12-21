using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatPicker : MonoBehaviour
{
    [SerializeField] bool m_variation = true;
    [SerializeField] bool m_randomStart = true;

    public delegate void OnFloatChange(float value);
    public OnFloatChange OnValueChange;

    public Slider m_slider;
    float m_totalValue;

    private void Awake()
    {
        m_slider = GetComponentInChildren<Slider>();
    }

    public void Initialize(float totalValue)
    {
        m_totalValue = totalValue;
        if (m_randomStart)
        {
            m_slider.value = Random.Range(0.0f, 1.0f);
        }
        else
        {
            if (m_variation)
                m_slider.value = 0.5f;
            else
                m_slider.value = 0.0f;
        }
    }

    public void OnChange()
    {
        float value = 0.0f;
        if (m_variation)
            value = ((m_slider.value - 0.5f) * m_totalValue) * 2.0f;
        else
            value = m_totalValue * m_slider.value;

        OnValueChange.Invoke(value);
    }
}
