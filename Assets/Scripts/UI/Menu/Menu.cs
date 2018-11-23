using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] TMP_InputField m_username = null;
    [SerializeField] TMP_InputField m_ipAddress = null;
    [SerializeField] LANToggle m_lanToggle = null;
    [SerializeField] PlayerColorPicker m_colorPicker = null;

    GraphicRaycaster m_raycaster;
    PointerEventData m_pointerEventData;
    EventSystem m_eventSystem;

    private void Start()
    {
        m_ipAddress.text = NomadNetworkManager.Instance.networkAddress;
        m_raycaster = GetComponent<GraphicRaycaster>();
        m_eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastUIHit();
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
        NomadNetworkManager.Instance.StartServer();
    }

    public void StartClient()
    {
        SetPlayerData();

        if (m_lanToggle.LAN)
        {
            NomadNetworkManager.Instance.networkAddress = m_ipAddress.text.Trim();
        }
        else
        {

        }

        NomadNetworkManager.Instance.StartClient();
    }

    public void StartHost()
    {
        SetPlayerData();
        NomadNetworkManager.Instance.StartHost();
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
}
