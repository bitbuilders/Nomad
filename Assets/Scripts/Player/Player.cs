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

    GameObject m_mainCamera;

    static int id = 0;

    private void Start()
    {
        m_mainCamera = Camera.main.gameObject;
        EnablePlayer();

        id++;

        if (isLocalPlayer)
        {
            RpcRecieveMessage("Worked");
        }
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

    [ClientRpc]
    public void RpcRecieveMessage(string msg)
    {
        Debug.Log(id + " recieved message: " + msg);
    }
}
