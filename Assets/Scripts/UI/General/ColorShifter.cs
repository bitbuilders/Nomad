using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorShifter : MonoBehaviour
{
    [SerializeField] Image m_image = null;
    [SerializeField] TextMeshProUGUI m_text = null;
    [SerializeField] bool m_useImage = true;
    [SerializeField] [Range(0.0f, 10.0f)] float m_colorShiftSpeed = 1.0f;
    [SerializeField] bool m_speedIsPerColor = true;
    [SerializeField] List<Color> m_colors = null;

    Gradient m_gradient;
    GradientColorKey[] m_colorKeys;
    GradientAlphaKey[] m_alphaKeys;
    float m_speed;

    private void Start()
    {
        m_gradient = new Gradient();

        m_colorKeys = new GradientColorKey[m_colors.Count];
        for (int i = 0; i < m_colorKeys.Length; i++)
        {
            m_colorKeys[i].color = m_colors[i];
            m_colorKeys[i].time = (float)i / (float)(m_colorKeys.Length - 1);
        }

        m_alphaKeys = new GradientAlphaKey[m_colors.Count];
        for (int i = 0; i < m_alphaKeys.Length; i++)
        {
            m_alphaKeys[i].alpha = m_colors[i].a;
            m_alphaKeys[i].time = (float)i / (float)(m_alphaKeys.Length - 1);
        }

        m_gradient.SetKeys(m_colorKeys, m_alphaKeys);
        m_gradient.mode = GradientMode.Blend;

        m_speed = (m_speedIsPerColor) ? m_colorShiftSpeed / m_colors.Count : m_colorShiftSpeed;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.realtimeSinceStartup * m_speed, 1.0f);
        Color c = m_gradient.Evaluate(t);
        if (m_useImage)
            m_image.color = c;
        else
            m_text.color = c;
    }
}
