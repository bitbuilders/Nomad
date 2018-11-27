using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dot : MonoBehaviour
{
    public enum DotState
    {
        FLY_IN,
        FLY_OUT,
        STAY,
        NONE
    }

    [SerializeField] [Range(0.0f, 10.0f)] float m_inSpeed = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_outSpeed = 0.5f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_linger = 1.0f;
    [SerializeField] [Range(0.0f, -500.0f)] float m_startHeight = -100.0f;
    [SerializeField] [Range(-100.0f, 100.0f)] float m_midHeight = 50.0f;
    [SerializeField] [Range(0.0f, 500.0f)] float m_endHeight = 100.0f;

    public DotState m_state { get; private set; }

    TextMeshProUGUI m_text;
    RectTransform m_rectTransform;
    float m_currentStayTime;
    float m_time;

    private void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_rectTransform = GetComponent<RectTransform>();
        ResetDot();
    }

    private void Update()
    {
        switch (m_state)
        {
            case DotState.FLY_IN:
                m_time += Time.deltaTime * m_inSpeed;
                float t = Interpolation.SmootherStep(m_time);
                float height = Mathf.Lerp(m_startHeight, m_midHeight, t);
                m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, height);
                float alpha = t * 2.0f;
                alpha = Mathf.Clamp01(alpha);
                UIJuice.Instance.SetTextAlpha(m_text, alpha);
                if (height >= m_midHeight)
                {
                    m_state = DotState.STAY;
                    m_time = 0.0f;
                }
                break;
            case DotState.FLY_OUT:
                m_time += Time.deltaTime * m_outSpeed;
                float t2 = Interpolation.SmootherStep(m_time);
                float height2 = Mathf.Lerp(m_midHeight, m_endHeight, t2);
                m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, height2);
                float alpha2 = 1.0f - (t2 * 2.0f);
                alpha2 = Mathf.Clamp01(alpha2);
                UIJuice.Instance.SetTextAlpha(m_text, alpha2);
                if (height2 >= m_endHeight)
                {
                    ResetDot();
                }
                break;
            case DotState.STAY:
                m_currentStayTime += Time.deltaTime;
                if (m_currentStayTime >= m_linger)
                {
                    m_state = DotState.FLY_OUT;
                }
                break;
        }
    }

    public void FlyIn()
    {
        ResetDot();
        m_state = DotState.FLY_IN;
    }

    public void FlyOut()
    {
        ResetDot();
        m_state = DotState.FLY_OUT;
    }

    public void ResetDot()
    {
        m_state = DotState.NONE;
        m_currentStayTime = 0.0f;
        m_time = 0.0f;
        m_rectTransform.anchoredPosition = new Vector2(m_rectTransform.anchoredPosition.x, m_startHeight);
        UIJuice.Instance.SetTextAlpha(m_text, 0.0f);
    }
}
