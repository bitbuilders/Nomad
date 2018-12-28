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
    [SerializeField] Camera m_camera = null;

    [SyncVar] public int ID;
    [SyncVar(hook = "OnChangeUsername")] public string UserName;
    [SyncVar(hook = "OnChangeColor")] string pColor;
    [SyncVar(hook = "OnChangeHairColor")] string hColor;
    [SyncVar(hook = "OnChangeGlassesColor")] string gColor;
    [SyncVar(hook = "OnChangeWeight")] float weight;
    [SyncVar(hook = "OnChangeHeight")] float height;
    public string Color { get { return Colors.ColorPrefix + pColor + ">"; } }
    public Camera Camera { get { return m_camera; } }

    GameObject m_mainCamera;
    ModelViewData m_modelViewData;
    
    private void Start()
    {
        m_mainCamera = Camera.main.gameObject;
        m_modelViewData = GetComponent<ModelViewData>();
        EnablePlayer();

        m_nametag.Initialize();

        if (isLocalPlayer)
        {
            LocalPlayerData.Instance.Initialize(this);
            GameLobby.Instance.LocalPlayerMovement = GetComponent<PlayerMovement>();
            CmdChangeUsername(LocalPlayerData.Instance.Attributes.Username);
            CmdChangeColor(LocalPlayerData.Instance.Attributes.Color);
            CmdChangeHairColor(Colors.ColorToString(LocalPlayerData.Instance.Attributes.Attributes.HairColor));
            CmdChangeGlassesColor(Colors.ColorToString(LocalPlayerData.Instance.Attributes.Attributes.GlassesColor));
            CmdChangeWeight(LocalPlayerData.Instance.Attributes.Attributes.WeightVariation);
            CmdChangeHeight(LocalPlayerData.Instance.Attributes.Attributes.HeightVariation);
            //CmdSendUsername(UserName);
            CmdGetID();
        }

        string name = (string.IsNullOrEmpty(UserName)) ? "Lost Nomad" : UserName;
        m_nametag.UpdateName(name);

        if (string.IsNullOrEmpty(pColor))
            pColor = "#C58D4D";
        m_nametag.UpdateColor(Colors.StringToColor(pColor));

        SetHairColor();
        SetGlassesColor();
        SetWeight();
        SetHeight();
    }

    void SetHairColor()
    {
        string color = (string.IsNullOrEmpty(hColor)) ? "#000000FF" : hColor;
        m_modelViewData.HairMaterial.color = Colors.StringToColor(color);
    }

    void SetGlassesColor()
    {
        string color = (string.IsNullOrEmpty(hColor)) ? "#000000FF" : gColor;
        m_modelViewData.GlassesMaterial.color = Colors.StringToColor(color);
    }

    void SetWeight()
    {
        float w = 1.0f + weight / 4.0f;
        transform.localScale = new Vector3(w, transform.localScale.y, w);
    }

    void SetHeight()
    {
        float h = 1.0f + height / 4.0f;
        transform.localScale = new Vector3(transform.localScale.x, h, transform.localScale.x);
    }

    [Command]
    void CmdGetID()
    {
        NetworkData nd = FindObjectOfType<NetworkData>();
        nd.IncrementPlayerID();
        ID = nd.CurrentPlayerID;
    }

    [Command]
    void CmdChangeHairColor(string color)
    {
        hColor = color;
        CmdSendHairColor(hColor);
    }

    void OnChangeHairColor(string color)
    {
        hColor = color;
        SetHairColor();
    }

    [Command]
    void CmdChangeGlassesColor(string color)
    {
        gColor = color;
        CmdSendGlassesColor(gColor);
    }

    void OnChangeGlassesColor(string color)
    {
        gColor = color;
        SetGlassesColor();
    }

    [Command]
    void CmdChangeWeight(float value)
    {
        weight = value;
        CmdSendWeight(weight);
    }

    void OnChangeWeight(float value)
    {
        weight = value;
        SetWeight();
    }

    [Command]
    void CmdChangeHeight(float value)
    {
        height = value;
        CmdSendHeight(height);
    }

    void OnChangeHeight(float value)
    {
        height = value;
        SetHeight();
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

    [Command]
    void CmdChangeColor(string color)
    {
        pColor = color;
        CmdSendColor(pColor);
    }

    void OnChangeColor(string color)
    {
        m_nametag.UpdateColor(Colors.StringToColor(color));
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
    void CmdSendHairColor(string color)
    {
        RpcReceiveHairColor(color);
    }

    [ClientRpc]
    void RpcReceiveHairColor(string color)
    {
        hColor = color;
        SetHairColor();
    }

    [Command]
    void CmdSendGlassesColor(string color)
    {
        RpcReceiveGlassesColor(color);
    }

    [ClientRpc]
    void RpcReceiveGlassesColor(string color)
    {
        gColor = color;
        SetGlassesColor();
    }

    [Command]
    void CmdSendWeight(float value)
    {
        RpcReceiveWeight(value);
    }

    [ClientRpc]
    void RpcReceiveWeight(float value)
    {
        weight = value;
        SetWeight();
    }

    [Command]
    void CmdSendHeight(float value)
    {
        RpcReceiveHeight(value);
    }

    [ClientRpc]
    void RpcReceiveHeight(float value)
    {
        height = value;
        SetHeight();
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

    [Command]
    void CmdSendColor(string color)
    {
        RpcReceiveColor(color);
    }

    [ClientRpc]
    void RpcReceiveColor(string color)
    {
        pColor = color;
    }

    private void OnApplicationQuit()
    {
        PartyManager pm = PartyManager.Instance;
        BillboardMessenger bbm = GetComponent<BillboardMessenger>();
        if (pm.LocalParty && pm.LocalParty.InParty)
            pm.LocalParty.LeaveParty();
        if (bbm.CurrentBillboard != null && bbm.CurrentBillboard.m_bbg.Playing)
            bbm.CurrentBillboard.LeaveBillboard();
        ChatRoomManager.Instance.LeaveAllRooms();
    }
}
