using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    [SerializeField] ToggleEvent m_onToggleShared = null;
    [SerializeField] ToggleEvent m_onToggleLocal = null;
    [SerializeField] ToggleEvent m_onToggleRemote = null;
    [SerializeField] float m_respawnTime = 5.0f;
    [SerializeField] Nametag m_nametag = null;

    [SyncVar(hook = "OnChangeUsername")] public string UserName;

    GameObject m_mainCamera;
    
    private void Start()
    {
        m_mainCamera = Camera.main.gameObject;
        EnablePlayer();

        m_nametag.Initialize();
        if (isLocalPlayer)
        {
            LocalPlayerData.Instance.Initialize(this);
            GameLobby.Instance.LocalPlayerMovement = GetComponent<PlayerMovement>();
            CmdChangeUsername(LocalPlayerData.Instance.TempUsername);
            //CmdSendUsername(UserName);
        }

        string name = (string.IsNullOrEmpty(UserName)) ? "Lost Nomad" : UserName;
        m_nametag.UpdateName(name);
    }

    [Command]
    void CmdChangeUsername(string username)
    {
        UserName = username;
        CmdSendUsername(UserName);
    }

    void OnChangeUsername(string username)
    {
        m_nametag.UpdateName(username);
    }
    
    void DisablePlayer()
    {
        if (isLocalPlayer)
            m_mainCamera.SetActive(true);

        m_onToggleShared.Invoke(false);

        if (isLocalPlayer)
            m_onToggleLocal.Invoke(false);
        else
            m_onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
            m_mainCamera.SetActive(false);

        m_onToggleShared.Invoke(true);

        if (isLocalPlayer)
            m_onToggleLocal.Invoke(true);
        else
            m_onToggleRemote.Invoke(true);
    }

    public void Die()
    {
        DisablePlayer();

        Invoke("Respawn", m_respawnTime);
    }

    void Respawn()
    {
        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        EnablePlayer();
    }

    public void EnableMovement(bool enable)
    {

    }

    [Command]
    void CmdSendUsername(string username)
    {
        RpcReceiveUsername(username);
    }

    [ClientRpc]
    void RpcReceiveUsername(string username)
    {
        UserName = username;
        gameObject.name = UserName;
    }
}
