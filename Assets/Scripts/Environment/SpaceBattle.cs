using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceBattle : BillboardGame
{
    [SerializeField] GameObject m_battleShip = null;
    [SerializeField] GameObject m_explosion = null;
    [SerializeField] GameObject m_powerup = null;
    [SerializeField] Sprite m_player1Sprite = null;
    [SerializeField] Sprite m_player2Sprite = null;
    [SerializeField] Sprite m_shotgunSprite = null;
    [SerializeField] Sprite m_bounceSprite = null;

    SpriteRenderer m_p1Sprite;
    SpriteRenderer m_p2Sprite;
    SpaceBattleShip m_p1Ship;
    SpaceBattleShip m_p2Ship;
    SpaceBattlePlayer m_player;
    RectTransform m_canvasRect;
    Vector2 m_canvasSize;
    public BillboardMessenger m_billboardMessenger;
    float m_lastFireTime;

    public override void Start()
    {
        m_player = GetComponent<SpaceBattlePlayer>();
        MaxPlayers = 2;
        Playing = false;

        m_p1Sprite = Instantiate(m_battleShip, Billboard.CanvasGame.transform).GetComponent<SpriteRenderer>();
        m_p2Sprite = Instantiate(m_battleShip, Billboard.CanvasGame.transform).GetComponent<SpriteRenderer>();
        m_p1Sprite.GetComponent<SpaceBattleShip>().Initialize(this, PlayerType.P1);
        m_p2Sprite.GetComponent<SpaceBattleShip>().Initialize(this, PlayerType.P2);
        m_p1Sprite.sprite = m_player1Sprite;
        m_p2Sprite.sprite = m_player2Sprite;
        m_p1Ship = m_p1Sprite.GetComponent<SpaceBattleShip>();
        m_p2Ship = m_p2Sprite.GetComponent<SpaceBattleShip>();
        ResetPlayerShips();
        m_canvasRect = Billboard.Canvas.GetComponent<RectTransform>();
        m_canvasSize = m_canvasRect.sizeDelta / 2.0f;
        m_lastFireTime = -m_player.m_fireRate;
        
        AudioManager.Instance.StartClipFromID("Space Battle Music", transform.position, false);
    }

    public override void Update()
    {
        base.Update();
        bool fire = Input.GetButton("Jump");

        float delta = Time.time - m_lastFireTime;

        if (Playing)
        {
            switch (PlayerNumber)
            {
                case PlayerType.P1:
                    m_p1Sprite.transform.localPosition += (Vector3)(m_player.Velocity * Time.deltaTime);
                    CheckOutsideBounds(m_p1Sprite);
                    if (fire && Playing && delta >= m_player.m_fireRate)
                    {
                        var fp = Fire(PlayerType.P1);
                        m_billboardMessenger.Fire(GameName.SPACE_BATTLE, PlayerType.P1, fp);
                        m_lastFireTime = Time.time;
                    }

                    break;
                case PlayerType.P2:
                    m_p2Sprite.transform.localPosition += (Vector3)(m_player.Velocity * Time.deltaTime);
                    CheckOutsideBounds(m_p2Sprite);
                    if (fire && Playing && delta >= m_player.m_fireRate)
                    {
                        var fp = Fire(PlayerType.P2);
                        m_billboardMessenger.Fire(GameName.SPACE_BATTLE, PlayerType.P2, fp);
                        m_lastFireTime = Time.time;
                    }

                    break;
            }
        }
    }

    public override void StartGame(PlayerType pt)
    {
        base.StartGame(pt);

        m_billboardMessenger = LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>();
    }

    public override void StopGame()
    {
        base.StopGame();
    }

    void ResetPlayerShips()
    {
        m_p1Sprite.transform.localPosition = new Vector2(-1.5f, 0.0f);
        m_p2Sprite.transform.localPosition = new Vector2(1.5f, 0.0f);
        m_p2Sprite.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
    }

    void CheckOutsideBounds(SpriteRenderer player)
    {
        float bbHeight = m_canvasSize.y;
        //float bbWidth = m_canvasSize.x;
        float heightLimit = bbHeight - (player.size.y / 2.0f);
        if (player.transform.localPosition.y > heightLimit)
        {
            m_player.Velocity = new Vector2(m_player.Velocity.x, -m_player.Velocity.y);
            player.transform.localPosition = new Vector2(player.transform.localPosition.x, heightLimit - 0.01f);
        }
        else if (player.transform.localPosition.y < -heightLimit)
        {
            m_player.Velocity = new Vector2(m_player.Velocity.x, -m_player.Velocity.y);
            player.transform.localPosition = new Vector2(player.transform.localPosition.x, -heightLimit + 0.01f);
        }
    }

    public void TakeDamage(PlayerType pt, Vector3 position)
    {
        PlayerType shooter = PlayerType.P1;
        if (pt == PlayerType.P1)
            shooter = PlayerType.P2;
        else
            shooter = PlayerType.P1;

        if (m_billboardMessenger)
        {
            m_billboardMessenger.DamagePlayer(shooter);
            m_billboardMessenger.SpawnExplosion(GameName.SPACE_BATTLE, pt, position);
        }
    }

    public void SpawnExplosion(PlayerType pt, Vector3 point)
    {
        Vector3 v = Vector3.zero;
        switch (pt)
        {
            case PlayerType.P1:
                v = m_p1Sprite.transform.position;
                break;
            case PlayerType.P2:
                v = m_p2Sprite.transform.position;
                break;
        }

        AudioManager.Instance.PlaySoundClip("Explosion", "Explosion", transform);
        GameObject go = Instantiate(m_explosion, point, Quaternion.identity, Billboard.CanvasGame.transform);
        Destroy(go, 2.0f);
    }

    public void SpawnPowerup(Vector3 position, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        GameObject go = Instantiate(m_powerup, transform);
        Sprite s = null;
        switch (powerup)
        {
            case SpaceBattleShipPowerup.PowerupType.BOUNCE:
                s = m_bounceSprite;
                break;
            case SpaceBattleShipPowerup.PowerupType.SHOTGUN:
                s = m_shotgunSprite;
                break;
        }
        go.GetComponent<SpaceBattlePowerup>().Initialize(this, id, powerup, s);
        go.transform.localPosition += position;
    }
    
    public void ObtainPowerup(PlayerType pt, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        m_billboardMessenger.GivePowerup(GameName.SPACE_BATTLE, pt, powerup, id);
    }

    public void GivePowerup(PlayerType pt, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        SpaceBattleShip sbs = null;
        switch (pt)
        {
            case PlayerType.P1:
                sbs = m_p1Ship;
                break;
            case PlayerType.P2:
                sbs = m_p2Ship;
                break;
        }

        SpaceBattlePowerup[] powerups = GetComponentsInChildren<SpaceBattlePowerup>();
        for (int i = 0; i < powerups.Length; i++)
        {
            if (powerups[i].ID == id)
            {
                Destroy(powerups[i].gameObject);
                break;
            }
        }

        sbs.GetComponent<SpaceBattleShipPowerup>().ObtainPowerup(powerup);
    }

    public override SpaceBattleShip.FirePosition Fire(PlayerType pt, SpaceBattleShip.FirePosition fp = SpaceBattleShip.FirePosition.NONE)
    {
        SpaceBattleShip ship = null;
        switch (pt)
        {
            case PlayerType.P1:
                ship = m_p1Ship;
                break;
            case PlayerType.P2:
                ship = m_p2Ship;
                break;
        }

        SpaceBattleShip.FirePosition point = SpaceBattleShip.FirePosition.NONE;
        point = ship.Fire(fp);
        return point;
    }

    public override void SetPlayerPosition(PlayerType pt, Vector3 position)
    {
        switch (pt)
        {
            case PlayerType.P1:
                m_p1Sprite.transform.position = position;
                break;
            case PlayerType.P2:
                m_p2Sprite.transform.position = position;
                break;
        }
    }

    public override void NetworkUpdate()
    {
        if (Playing)
        {
            switch (PlayerNumber)
            {
                case PlayerType.P1:
                    m_billboardMessenger.SetPlayerPosition(GameName.SPACE_BATTLE, PlayerType.P1, m_p1Sprite.transform.position);
                    break;
                case PlayerType.P2:
                    m_billboardMessenger.SetPlayerPosition(GameName.SPACE_BATTLE, PlayerType.P2, m_p2Sprite.transform.position);
                    break;
            }
        }
    }

    void ShowScore()
    {
        Billboard.ShowScoreText();
    }
}
