using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameManager : Singleton<PlayerGameManager>
{
    [SerializeField] GameObject m_playerGameTemplate = null;
    [SerializeField] Transform m_playerGameLocation = null;
    [SerializeField] GameObject m_playerGameView = null;
    [SerializeField] [Range(0.0f, 30.0f)] float m_automaticRefreshRate = 10.0f;

    List<PlayerGame> m_playerGames;
    float m_refreshTime;

    private void Start()
    {
        m_playerGames = new List<PlayerGame>();
        m_refreshTime = 0.0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        m_refreshTime += Time.deltaTime;
        if (m_refreshTime >= m_automaticRefreshRate)
        {
            m_refreshTime = 0.0f;
            RefreshList();
        }
    }

    public PlayerGame CreatePlayerGame(string ip, string hostName)
    {
        PlayerGame existingGame = GetGame(ip, hostName);
        if (existingGame != null)
            return existingGame;

        GameObject go = Instantiate(m_playerGameTemplate, Vector3.zero, Quaternion.identity, m_playerGameLocation);
        PlayerGame pg = go.GetComponent<PlayerGame>();
        bool startHidden = m_playerGameView.activeInHierarchy ? false : true;
        pg.Initialize(ip, hostName, startHidden);
        m_playerGames.Add(pg);

        return pg;
    }

    public PlayerGame GetGame(string ip, string hostName)
    {
        PlayerGame game = null;

        foreach (PlayerGame pg in m_playerGames)
        {
            if (pg.IP == ip && pg.HostName == hostName)
            {
                game = pg;
                break;
            }
        }

        return game;
    }

    public void RefreshList()
    {
        PlayerGame[] games = m_playerGames.ToArray();
        for (int i = 0; i < games.Length; i++)
        {
            Destroy(games[i].gameObject);
        }

        m_playerGames.Clear();
    }

    public void FadePlayerGames(bool fadeIn)
    {
        foreach (PlayerGame pg in m_playerGames)
        {
            pg.Fade(fadeIn);
        }
    }
}
