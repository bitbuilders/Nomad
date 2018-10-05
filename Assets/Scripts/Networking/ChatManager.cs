using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatManager : MonoBehaviour
{
    [SerializeField] NetworkManager m_networkManager = null;
    [SerializeField] InputField m_input = null;

    void Start()
    {
        m_networkManager.StartHost();
        m_input.Init();
    }
    
    void Update()
    {

    }
}
