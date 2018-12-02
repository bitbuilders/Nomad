using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillboardText : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_lingerTime = 1.0f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeInTime = 0.25f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeOutTime = 0.25f;

    TextMeshProUGUI m_text;
    float m_time;

    private void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_time = m_lingerTime + m_fadeInTime + m_fadeOutTime;
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        float a = 0.0f;
        if (m_time < m_fadeInTime + m_lingerTime + m_fadeOutTime)
        {
            if (m_time < m_fadeInTime)
                a = m_time / m_fadeInTime;
            else if (m_time > m_fadeInTime + m_lingerTime)
            {
                a = 1.0f - (m_time - (m_fadeInTime + m_lingerTime)) / m_fadeOutTime;
            }
            else
                a = 1.0f;
        }

        UIJuice.Instance.SetTextAlpha(m_text, a);
    }

    public void SetText(string text)
    {
        m_text.text = text;
    }

    public void BlinkText()
    {
        m_time = 0.0f;
    }
}
