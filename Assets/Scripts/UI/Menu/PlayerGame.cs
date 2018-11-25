using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_hostNameText = null;
    [SerializeField] List<TextMeshProUGUI> m_textToFade = null;
    [SerializeField] List<Image> m_imagesToFade = null;

    public string IP { get; set; }
    public string HostName { get; set; }

    public void Initialize(string ip, string hostName)
    {
        IP = ip;
        HostName = hostName;

        UpdateText();
    }

    public void Join()
    {
        Menu.Instance.StartClient(IP);
    }

    void UpdateText()
    {
        m_hostNameText.text = HostName;
    }

    public void Fade(bool fadeIn)
    {
        float a = (fadeIn) ? 1.0f : 0.0f;
        UIJuice.Instance.FadeAlpha(m_imagesToFade, a, fadeIn, 0.2f, false);
        UIJuice.Instance.FadeText(m_textToFade, a, fadeIn, 0.2f, false);
    }
}
