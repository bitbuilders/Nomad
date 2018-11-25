using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameManager : Singleton<PlayerGameManager>
{
    [SerializeField] GameObject m_playerGameTemplate = null;
    [SerializeField] Transform m_playerGameLocation = null;

    public void CreatePlayerGame(string ip, string hostName)
    {
        GameObject go = Instantiate(m_playerGameTemplate, Vector3.zero, Quaternion.identity, m_playerGameLocation);
        PlayerGame pg = go.GetComponent<PlayerGame>();
        pg.Initialize(ip, hostName);
    }
}
