using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattle : BillboardGame
{
    [SerializeField] GameObject m_playerSprite = null;

    RectTransform m_p1Sprite;
    RectTransform m_p2Sprite;
    SpaceBattlePlayer m_player;

    public override void Start()
    {
        m_player = GetComponent<SpaceBattlePlayer>();
        MaxPlayers = 2;
        Playing = false;

        m_p1Sprite = Instantiate(m_playerSprite, Billboard.Canvas.transform).GetComponent<RectTransform>();
        m_p2Sprite = Instantiate(m_playerSprite, Billboard.Canvas.transform).GetComponent<RectTransform>();
        ResetPlayerShips();
    }

    private void Update()
    {
        switch (PlayerNumber)
        {
            case PlayerType.P1:
                m_p1Sprite.anchoredPosition += m_player.Velocity * Time.deltaTime;
                break;
            case PlayerType.P2:

                break;
        }
    }

    public override void StartGame(PlayerType pt)
    {
        base.StartGame(pt);


    }

    public override void StopGame()
    {
        base.StopGame();
    }

    void ResetPlayerShips()
    {
        m_p1Sprite.anchoredPosition = new Vector2(-1.5f, 0.0f);
        m_p2Sprite.anchoredPosition = new Vector2(1.5f, 0.0f);
    }
}
