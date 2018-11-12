using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteSlice : MonoBehaviour
{
    [SerializeField] Color m_normalColor;
    [SerializeField] Color m_highlightColor;

    public Emote Emote;

    EmoteSelector m_emoteSelector = null;
    Image m_image;
    bool m_fadeIn;
    float m_time;
    float m_fadeInTime;
    float m_fadeOutTime;

    private void Start()
    {
        m_emoteSelector = GetComponentInParent<EmoteSelector>();
        m_image = GetComponent<Image>();

        m_fadeInTime = m_emoteSelector.ColorTransitionInTime;
        m_fadeOutTime = m_emoteSelector.ColorTransitionOutTime;
    }

    private void Update()
    {
        if (m_fadeIn)
        {
            Fade(true);
        }
        else
        {
            Fade(false);
        }
    }

    void Fade(bool fadeIn)
    {
        m_time += Time.deltaTime;
        m_time = Mathf.Clamp(m_time, 0.0f, m_fadeInTime);
        float t = (fadeIn) ? m_time / m_fadeInTime : 1.0f - (m_time / m_fadeOutTime);
        Color c = Color.Lerp(m_normalColor, m_highlightColor, t);
        m_image.color = new Color(c.r, c.g, c.b, m_image.color.a);
    }

    public void OnMouseHover()
    {
        m_emoteSelector.CurrentEmote = Emote.Name;
        m_fadeIn = true;
        m_time = 0.0f;
    }

    public void OnMouseLeave()
    {
        m_emoteSelector.CurrentEmote = "None";
        m_fadeIn = false;
        m_time = 0.0f;
    }
}
