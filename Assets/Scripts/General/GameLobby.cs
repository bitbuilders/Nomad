using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobby : Singleton<GameLobby>
{
    [SerializeField] GameObject m_hubMenu = null;
    [SerializeField] GameObject m_dmMenu = null;
    [SerializeField] NomadNetworkDiscovery m_networkDiscovery = null;

    public bool HUBOpen { get { return !m_HUB.Hidden; } }
    public bool DMOpen { get { return !m_DM.Hidden; } }
    public PlayerMovement LocalPlayerMovement;

    HUBMenu m_HUB;
    DirectMessageInterface m_DM;
    PlayerMovement.PlayerState m_noInputState;

    private void Start()
    {
        m_HUB = m_hubMenu.GetComponent<HUBMenu>();
        m_DM = m_dmMenu.GetComponent<DirectMessageInterface>();
        m_noInputState = PlayerMovement.PlayerState.CHAT_ROOM | PlayerMovement.PlayerState.PARTY_MESSAGE;

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);

        m_networkDiscovery.Initialize();
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.isServer)
        {
            m_networkDiscovery.StartAsServer();
            string name = LocalPlayerData.Instance.TempUsername;
            m_networkDiscovery.broadcastData = (string.IsNullOrEmpty(name)) ? "Lost Nomad" : name;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ToggleMenu();
        }
        if (Input.GetButtonDown("DM") && !LocalPlayerMovement.ContainsStates(m_noInputState))
        {
            ToggleDMMenu();
        }
    }

    public void ToggleMenu()
    {
        m_HUB.ToggleVisibility();
    }

    public void ToggleDMMenu()
    {
        m_DM.ToggleVisibility();
    }

    public void QuitGame()
    {
        Quit();
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
