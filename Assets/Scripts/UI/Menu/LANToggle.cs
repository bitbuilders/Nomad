using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MenuToggle : UnityEvent<bool> { }

public class LANToggle : MonoBehaviour
{
    [SerializeField] MenuToggle m_lanModeToggle = null;
    [SerializeField] MenuOptions m_menuOptions = null;
    [SerializeField] NomadNetworkDiscovery m_networkDiscovery = null;
    [SerializeField] MassFader m_gamesViewFader = null;
    [SerializeField] [Range(0.0f, 3.0f)] float m_cooldown = 0.3f;

    public bool LAN { get { return m_enabled; } }

    bool m_enabled;
    float m_time;
    UIButton m_button;
    PlayerGameManager m_playerGameManager;

    private void Awake()
    {
        m_networkDiscovery.Initialize();
        m_playerGameManager = PlayerGameManager.Instance;
    }

    private void Start()
    {
        m_button = GetComponent<UIButton>();
        m_enabled = false;
        m_time = -m_cooldown;

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);
        m_networkDiscovery.StartAsClient();
    }

    public void Toggle()
    {
        float delta = Time.time - m_time;
        if (delta < m_cooldown)
            return;
        m_time = Time.time;

        m_enabled = !m_enabled;

        if (m_enabled)
        {
            EnableLAN();
        }
        else
        {
            DisableLAN();
        }

        m_menuOptions.ToggleSize();
        m_button.Swell();
    }

    void EnableLAN()
    {
        m_lanModeToggle.Invoke(true);
        m_gamesViewFader.gameObject.SetActive(true);
        m_gamesViewFader.FadeIn();
        m_playerGameManager.FadePlayerGames(true);
    }

    void DisableLAN()
    {
        m_lanModeToggle.Invoke(false);
        m_gamesViewFader.FadeOut(false, true);
        m_playerGameManager.FadePlayerGames(false);
    }
}
