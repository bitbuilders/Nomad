using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_playerName = null;

    public void Initialize(string playerName)
    {
        m_playerName.text = playerName;
    }
}
