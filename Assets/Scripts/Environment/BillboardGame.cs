using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BillboardGame : MonoBehaviour
{
    public enum PlayerType
    {
        P1,
        P2,
        P3,
        P4
    }

    public int MaxPlayers { get; protected set; }
    public bool Playing { get; protected set; }
    public Billboard Billboard { get; protected set; }
    public PlayerType PlayerNumber { get; protected set; }

    public virtual void Start()
    {
        MaxPlayers = 2;
    }

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
}
