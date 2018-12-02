using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BillboardMessenger : NetworkBehaviour
{
    public BillboardGame.GameName CurrentGame { get; private set; }

    public void PlayBillboard(BillboardGame.GameName game)
    {
        CurrentGame = game;
        CmdAddPlayer();
        CmdInitializeScores();
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
    void CmdAddPlayer()
    {
        GetBillboardFromName().m_players++;
        GetBillboardFromName().m_nextPlayer++;
    }

    public void LeaveBillboard(BillboardGame.PlayerType pt)
    {
        CmdRmovePlayer(pt);
    }

    [Command]
    void CmdRmovePlayer(BillboardGame.PlayerType pt)
    {
        Billboard bb = GetBillboardFromName();
        bb.m_players--;
        int left = (int)pt;
        if (bb.m_nextPlayer > left)
            bb.m_nextPlayer = left;
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

    Billboard GetBillboardFromName()
    {
        return GetBillboardFromName(CurrentGame);
    }

    Billboard GetBillboardFromName(BillboardGame.GameName game)
    {
        return BillboardManager.Instance.GetBillboard(game);
    }
}
