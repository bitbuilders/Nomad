using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteSelector : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_emoteFadeInTime = 0.2f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_emoteFadeOutTime = 0.2f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_cooldown = 0.3f;
    [Range(0.0f, 5.0f)] public float ColorTransitionInTime = 0.15f;
    [Range(0.0f, 5.0f)] public float ColorTransitionOutTime = 0.15f;
    [SerializeField] Canvas m_canvas;
    [SerializeField] List<Image> m_sliceImages;
    
    public string CurrentEmote { get; set; }

    PlayerMovement.PlayerState m_cannotEmoteState;
    float m_time;
    float m_cooldownTime;
    float m_size;
    bool m_fadeIn;
    bool m_fade;

    private void Start()
    {
        SetImageAlpha(0.0f);
        SetRaycastActive(false);
        m_fadeIn = false;
        m_fade = false;
        m_cooldownTime = m_cooldown;

        RectTransform childWidthAndHeight = GetComponentInChildren<EmoteSlice>().GetComponent<RectTransform>();
        m_size = childWidthAndHeight.sizeDelta.x;

        m_cannotEmoteState = (PlayerMovement.PlayerState.CHAT_ROOM | PlayerMovement.PlayerState.DIRECT_MESSAGE | PlayerMovement.PlayerState.PARTY_MESSAGE | PlayerMovement.PlayerState.DIALOG);
    }

    private void Update()
    {
        m_cooldownTime += Time.deltaTime;

        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        PlayerMovement playerMove = null;
        if (localPlayer)
            playerMove = localPlayer.GetComponent<PlayerMovement>();

        if (Input.GetButtonDown("Emote") && !playerMove.ContainsStates(m_cannotEmoteState))
        {
            SetRaycastActive(true);
            m_fadeIn = true;
            m_fade = true;
            
            playerMove.AddState(PlayerMovement.PlayerState.EMOTE);
            SetWheelPosition();
        }
        else if (Input.GetButtonUp("Emote") && m_fadeIn && m_cooldownTime >= m_cooldown)
        {
            m_cooldownTime = 0.0f;
            SetRaycastActive(false);
            m_fadeIn = false;
            SendEmote();
            m_fade = true;
            
            playerMove.RemoveState(PlayerMovement.PlayerState.EMOTE);
        }
        else if (Input.GetButtonUp("Emote") && m_fadeIn)
        {
            m_fade = true;
            m_fadeIn = false;
            playerMove.RemoveState(PlayerMovement.PlayerState.EMOTE);
            SetRaycastActive(false);
        }

        if (m_fade)
        {
            float duration = (m_fadeIn) ? m_emoteFadeInTime : m_emoteFadeOutTime;
            Fade(m_fadeIn, duration);
        }
    }

    void SetWheelPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        float scaledSize = m_size * m_canvas.scaleFactor + 5.0f;
        mousePos.x = Mathf.Clamp(mousePos.x, scaledSize, Screen.width - scaledSize);
        mousePos.y = Mathf.Clamp(mousePos.y, scaledSize, Screen.height - scaledSize);
        transform.position = mousePos;
    }

    void Fade(bool fadeIn, float duration)
    {
        m_time += Time.deltaTime;
        m_time = Mathf.Clamp(m_time, 0.0f, duration);
        float t = (fadeIn) ? m_time / duration : 1.0f - (m_time / duration);
        SetImageAlpha(t);

        if (m_time == duration)
        {
            m_fade = false;
            m_time = 0.0f;
        }
    }

    void SendEmote()
    {
        if (string.IsNullOrEmpty(CurrentEmote))
            return;

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
}
