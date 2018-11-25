using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameManager : Singleton<PlayerGameManager>
{
    [SerializeField] GameObject m_playerGameTemplate = null;
    [SerializeField] Transform m_playerGameLocation = null;

    List<PlayerGame> m_playerGames;

    private void Start()
    {
        m_playerGames = new List<PlayerGame>();
    }

    public void CreatePlayerGame(string ip, string hostName)
    {
        if (GameExists(ip, hostName))
            return;

        GameObject go = Instantiate(m_playerGameTemplate, Vector3.zero, Quaternion.identity, m_playerGameLocation);
        PlayerGame pg = go.GetComponent<PlayerGame>();
        pg.Initialize(ip, hostName);
        m_playerGames.Add(pg);
    }

    public bool GameExists(string ip, string hostName)
    {
        bool exists = false;

        foreach (PlayerGame pg in m_playerGames)
        {
            if (pg.IP == ip && pg.HostName == hostName)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }

    public void RefreshList()
    {
        m_playerGames.Clear();

        PlayerGame[] games = m_playerGameLocation.GetComponentsInChildren<PlayerGame>();
        for (int i = 0; i < games.Length; i++)
        {
            Destroy(games[i].gameObject);
        }
    }

    public void FadePlayerGames(bool fadeIn)
    {
        foreach (PlayerGame pg in m_playerGames)
        {
            pg.Fade(fadeIn);
        }
    }
}
