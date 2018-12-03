using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BillboardMessenger : NetworkBehaviour
{
    public BillboardGame.GameName CurrentGame { get; private set; }
    public Billboard CurrentBillboard;
    SpaceBattlePowerupSpawner m_powerupSpawner;

    private void Start()
    {
        if (isServer)
            m_powerupSpawner = GameObject.Find("SpaceBattlePowerupSpawner").GetComponent<SpaceBattlePowerupSpawner>();
    }

    public void PlayBillboard(BillboardGame.GameName game, BillboardGame.PlayerType pt, int playerID)
    {
        CurrentGame = game;
        CurrentBillboard = GetBillboardFromName();
        CmdInitializeScores();
        CmdAddPlayer(pt, playerID);
    }

    [Command]
    void CmdInitializeScores()
    {
        Billboard bb = GetBillboardFromName();
        if (bb.m_playerScores.Count == 0)
            bb.InitializeScores();
        RpcShowLocalScore();
    }

    [ClientRpc]
    void RpcShowLocalScore()
    {
        if (isLocalPlayer)
            GetBillboardFromName().ShowScoreText();
    }

    [Command]
    void CmdAddPlayer(BillboardGame.PlayerType pt, int playerID)
    {
        Billboard bb = GetBillboardFromName();
        bb.AddPlayer(pt, playerID);
        bb.SetNextPlayer();
    }

    public void LeaveBillboard(BillboardGame.PlayerType pt)
    {
        CmdRmovePlayer(pt);
    }

    [Command]
    void CmdRmovePlayer(BillboardGame.PlayerType pt)
    {
        Billboard bb = GetBillboardFromName();
        bb.RemovePlayer(pt);
        bb.SetNextPlayer();
    }

    public void Fire(BillboardGame.GameName game, BillboardGame.PlayerType pt, SpaceBattleShip.FirePosition fp)
    {
        CmdFire(game, pt, fp);
    }

    [Command]
    void CmdFire(BillboardGame.GameName game, BillboardGame.PlayerType pt, SpaceBattleShip.FirePosition fp)
    {
        RpcFire(game, pt, fp);
    }

    [ClientRpc]
    void RpcFire(BillboardGame.GameName game, BillboardGame.PlayerType pt, SpaceBattleShip.FirePosition fp)
    {
        if (!isLocalPlayer)
        {
            Billboard bb = GetBillboardFromName(game);
            if (bb)
                bb.m_bbg.Fire(pt, fp);
        }
    }

    public void SetPlayerPosition(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 position)
    {
        CmdSetPosition(game, pt, position);
    }

    [Command]
    void CmdSetPosition(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 position)
    {
        RpcSetPosition(game, pt, position);
    }

    [ClientRpc]
    void RpcSetPosition(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 position)
    {
        if (!isLocalPlayer)
        {
            Billboard bb = GetBillboardFromName(game);
            if (bb)
                bb.m_bbg.SetPlayerPosition(pt, position);
        }
    }

    public void DamagePlayer(BillboardGame.PlayerType pt)
    {
        CmdSetScore(pt);
    }

    [Command]
    void CmdSetScore(BillboardGame.PlayerType pt)
    {
        GetBillboardFromName().AddScore(pt);
        RpcShowScore();
    }

    [ClientRpc]
    void RpcShowScore()
    {
        GetBillboardFromName().ShowScoreText();
    }

    public void SpawnExplosion(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 point)
    {
        CmdSpawnExplosion(game, pt, point);
    }

    [Command]
    void CmdSpawnExplosion(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 point)
    {
        RpcSpawnExplosion(game, pt, point);
    }

    [ClientRpc]
    void RpcSpawnExplosion(BillboardGame.GameName game, BillboardGame.PlayerType pt, Vector3 point)
    {
        switch (game)
        {
            case BillboardGame.GameName.SPACE_BATTLE:
                ((SpaceBattle)GetBillboardFromName().m_bbg).SpawnExplosion(pt, point);
                break;
        }
    }

    public void GivePowerup(BillboardGame.GameName game, BillboardGame.PlayerType player, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        CmdGivePowerup(game, player, powerup, id);
    }

    [Command]
    void CmdGivePowerup(BillboardGame.GameName game, BillboardGame.PlayerType player, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        RpcGivePowerup(game, player, powerup, id);
    }

    [ClientRpc]
    void RpcGivePowerup(BillboardGame.GameName game, BillboardGame.PlayerType player, SpaceBattleShipPowerup.PowerupType powerup, int id)
    {
        switch (game)
        {
            case BillboardGame.GameName.SPACE_BATTLE:
                ((SpaceBattle)GetBillboardFromName().m_bbg).GivePowerup(player, powerup, id);
                break;
        }
    }

    public void SpawnPowerup(BillboardGame.GameName game, SpaceBattleShipPowerup.PowerupType powerup, Vector3 position, int id)
    {
        CmdSpawnPowerup(game, powerup, position, id);
    }

    [Command]
    void CmdSpawnPowerup(BillboardGame.GameName game, SpaceBattleShipPowerup.PowerupType powerup, Vector3 position, int id)
    {
        switch (game)
        {
            case BillboardGame.GameName.SPACE_BATTLE:
                m_powerupSpawner.m_powerupID++;
                break;
        }
        RpcSpawnPowerup(game, powerup, position, id);
    }

    [ClientRpc]
    void RpcSpawnPowerup(BillboardGame.GameName game, SpaceBattleShipPowerup.PowerupType powerup, Vector3 position, int id)
    {
        switch (game)
        {
            case BillboardGame.GameName.SPACE_BATTLE:
                ((SpaceBattle)GetBillboardFromName().m_bbg).SpawnPowerup(position, powerup, id);
                break;
        }
    }

    Billboard GetBillboardFromName()
    {
        return GetBillboardFromName(CurrentGame);
    }

    Billboard GetBillboardFromName(BillboardGame.GameName game)
    {
        return BillboardManager.Instance.GetBillboard(game);
    }
}
