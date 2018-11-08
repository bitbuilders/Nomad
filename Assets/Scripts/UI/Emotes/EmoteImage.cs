using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteImage : MonoBehaviour
{
    EmoteSlice m_emoteSlice;
    Image m_emoteImage;

    private void Start()
    {
        m_emoteSlice = GetComponentInParent<EmoteSlice>();
        m_emoteImage = GetComponent<Image>();

        m_emoteImage.sprite = m_emoteSlice.Emote.Sprite;
    }
}
