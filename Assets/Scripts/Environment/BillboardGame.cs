using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BillboardGame : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] protected float m_networkSendRate = 0.112f;

    public enum PlayerType
    {
        P1,
        P2,
        P3,
        P4
    }

    public enum GameName
    {
        SPACE_BATTLE
    }

    public int MaxPlayers { get; protected set; }
    public bool Playing { get; protected set; }
    public Billboard Billboard { get; protected set; }
    public PlayerType PlayerNumber { get; protected set; }

    protected float m_networkTime;

    public virtual void Start()
    {
        MaxPlayers = 2;
    }

    public virtual void Update()
    {
        m_networkTime += Time.deltaTime;

        if (m_networkTime >= m_networkSendRate)
        {
            m_networkTime = 0.0f;
            NetworkUpdate();
        }
    }

    public abstract void NetworkUpdate();

    public virtual void Initialize(Billboard bb)
    {
        Billboard = bb;
    }

    public virtual void StartGame(PlayerType pt)
    {
        PlayerNumber = pt;
        Playing = true;
    }

    public virtual void StopGame()
    {
        Playing = false;
    }

    public virtual SpaceBattleShip.FirePosition Fire(PlayerType pt, SpaceBattleShip.FirePosition fp)
    {
        return SpaceBattleShip.FirePosition.NONE;
    }

    public virtual void SetPlayerPosition(PlayerType pt, Vector3 position)
    {

    }
}
