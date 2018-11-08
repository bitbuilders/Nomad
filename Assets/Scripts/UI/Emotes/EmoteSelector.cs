using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteSelector : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_emoteFadeInTime = 0.2f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_emoteFadeOutTime = 0.2f;
    [Range(0.0f, 5.0f)] public float ColorTransitionInTime = 0.15f;
    [Range(0.0f, 5.0f)] public float ColorTransitionOutTime = 0.15f;
    [SerializeField] List<Image> m_sliceImages;


    public string CurrentEmote { get; set; }

    private void Start()
    {
        SetImageAlpha(0.0f);
        SetRaycastActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Emote"))
        {
            SetRaycastActive(true);
            Fade(true, m_emoteFadeInTime);
        }
        else if (Input.GetButtonUp("Emote"))
        {
            SetRaycastActive(false);
            Fade(false, m_emoteFadeOutTime);
            SendEmote();
        }
    }

    void SendEmote()
    {
        string emote = CurrentEmote.Trim();
        if (!string.IsNullOrEmpty(emote) && emote != "None")
            LocalPlayerData.Instance.LocalPlayer.GetComponent<EmoteMessenger>().Emote(emote);
    }

    public void SetRaycastActive(bool active)
    {
        foreach (Image i in m_sliceImages)
        {
            i.raycastTarget = active;
        }
    }

    void SetImageAlpha(float alpha)
    {
        foreach (Image i in m_sliceImages)
        {
            Color c = i.color;
            c.a = alpha;
            i.color = c;
        }
    }

    public void Fade(bool fadeIn, float time)
    {
        // TODO: Use fading within this class with a toggle (doesn't work atm when quickly tapping emote button)
        UIJuice.Instance.FadeAlpha(m_sliceImages, fadeIn, time);
    }
}
