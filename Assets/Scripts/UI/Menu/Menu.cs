using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : Singleton<Menu>
{
    [SerializeField] TMP_InputField m_username = null;
    [SerializeField] TMP_InputField m_ipAddress = null;
    [SerializeField] LANToggle m_lanToggle = null;
    [SerializeField] PlayerColorPicker m_colorPicker = null;
    [SerializeField] QuitDialog m_quitDialog = null;
    [SerializeField] [Range(0.0f, 5.0f)] float m_connectCooldown = 1.0f;

    GraphicRaycaster m_raycaster;
    PointerEventData m_pointerEventData;
    EventSystem m_eventSystem;
    float m_connectTime;

    private void Start()
    {
        m_ipAddress.text = NomadNetworkManager.Instance.networkAddress;
        m_raycaster = GetComponent<GraphicRaycaster>();
        m_eventSystem = FindObjectOfType<EventSystem>();
        m_connectTime = -m_connectCooldown;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastUIHit();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            m_quitDialog.Show();
        }
    }

    void RaycastUIHit()
    {
        m_pointerEventData = new PointerEventData(m_eventSystem);
        m_pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> rayResults = new List<RaycastResult>();
        m_raycaster.Raycast(m_pointerEventData, rayResults);
        bool hitColorPicker = false;
        foreach (RaycastResult result in rayResults)
        {
            if (result.gameObject == m_colorPicker.gameObject || result.gameObject.transform.IsChildOf(m_colorPicker.gameObject.transform))
            {
                hitColorPicker = true;
                m_colorPicker.OpenColors();
                break;
            }
        }

        if (!hitColorPicker)
            m_colorPicker.CloseColors();
    }

    public void StartServer()
    {
        float delta = Time.time - m_connectTime;
        if (delta < m_connectCooldown)
            return;
        m_connectTime = Time.time;

        NomadNetworkManager.Instance.StartServer();
    }

    public void StartClient(string ip = "")
    {
        float delta = Time.time - m_connectTime;
        if (delta < m_connectCooldown)
            return;
        m_connectTime = Time.time;

        SetPlayerData();
        
        if (m_lanToggle.LAN && string.IsNullOrEmpty(ip))
        {
            NomadNetworkManager.Instance.networkAddress = m_ipAddress.text.Trim();
        }
        else
        {
            NomadNetworkManager.Instance.networkAddress = ip.Trim();
        }

        string address = NomadNetworkManager.Instance.networkAddress;
        print("Connecting to IP: " + address);
        NomadNetworkManager.Instance.StartClient();
        ConnectionManager.Instance.ShowConnectionMessage(address);
    }

    public void StartHost()
    {
        float delta = Time.time - m_connectTime;
        if (delta < m_connectCooldown)
            return;
        m_connectTime = Time.time;

        SetPlayerData();
        NomadNetworkManager.Instance.StartHost();
        ConnectionManager.Instance.ShowHostMessage();
    }

    void SetPlayerData()
    {
        string username = m_username.text.Trim();
        if (!string.IsNullOrEmpty(username))
            LocalPlayerData.Instance.TempUsername = username;
        else
            LocalPlayerData.Instance.TempUsername = "Lost Nomad";

        LocalPlayerData.Instance.TempColor = m_colorPicker.Color;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
