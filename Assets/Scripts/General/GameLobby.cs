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
    PlayerMovement m_playerMovement;
    NomadNetworkManager m_networkManager;

    private void Start()
    {
        AudioManager.Instance.StopClipFromID("Menu Music", false);
        AudioManager.Instance.StartClipFromID("Lobby Music", Vector3.zero, false);
        m_networkManager = FindObjectOfType<NomadNetworkManager>();
        m_HUB = m_hubMenu.GetComponent<HUBMenu>();
        m_DM = m_dmMenu.GetComponent<DirectMessageInterface>();
        m_noInputState = PlayerMovement.PlayerState.CHAT_ROOM | PlayerMovement.PlayerState.PARTY_MESSAGE;

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);

        string name = LocalPlayerData.Instance.Attributes.Username;
        m_networkDiscovery.broadcastData = (string.IsNullOrEmpty(name)) ? "Lost Nomad" : name;
        m_networkDiscovery.Initialize();
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        m_playerMovement = localPlayer.GetComponent<PlayerMovement>();
        if (localPlayer && localPlayer.isServer)
        {
            m_networkDiscovery.StartAsServer();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!m_playerMovement.ContainsStates(PlayerMovement.PlayerState.GAME))
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

    public void ReturnToMainMenu()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        if (localPlayer.isClient && localPlayer.isServer)
            m_networkManager.StopHost();
        else if (localPlayer.isServer)
            m_networkManager.StopServer();
        else
            m_networkManager.StopClient();

        if (m_networkDiscovery.running)
        {
            m_networkDiscovery.StopBroadcast();
            Destroy(m_networkDiscovery.gameObject);
        }
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
