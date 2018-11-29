using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Billboard : NetworkBehaviour
{
    [SerializeField] GameObject m_billboardGame = null;
    [SerializeField] Canvas m_canvas = null;
    [SerializeField] Canvas m_mainUICanvas;
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 3.0f;
    [SerializeField] Transform m_cameraPosition = null;

    public Canvas Canvas { get { return m_canvas; } }

    [SyncVar] public int m_players = 0;
    BillboardGame m_bbg;

    private void Start()
    {
        GameObject go = Instantiate(m_billboardGame, transform);
        m_bbg = go.GetComponent<BillboardGame>();
        m_bbg.Initialize(this);
    }

    private void Update()
    {
        if (Input.GetButtonDown("PlayBillboard"))
        {
            Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
            Vector3 dir = localPlayer.transform.position - transform.position;
            if (dir.magnitude <= m_interactionDistance)
            {
                if (m_players < m_bbg.MaxPlayers)
                {
                    PlayBillboard();
                }
                else
                {
                    // Error message

                }
            }
        }
        else if (Input.GetButtonDown("Cancel") && m_bbg.Playing)
        {
            LeaveBillboard();
        }
    }

    public void PlayBillboard()
    {
        BillboardGame.PlayerType pt = (BillboardGame.PlayerType)m_players;
        m_bbg.StartGame(pt);
        CmdPlay();
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<PlayerMovement>().AddState(PlayerMovement.PlayerState.GAME);
        localPlayer.Camera.GetComponent<CameraMovement>().TargetPoint = m_cameraPosition;
        m_mainUICanvas.gameObject.SetActive(false);
    }

    [Command]
    void CmdPlay()
    {
        m_players++;
    }

    public void LeaveBillboard()
    {
        m_bbg.StopGame();
        CmdLeave();
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<PlayerMovement>().RemoveState(PlayerMovement.PlayerState.GAME);
        m_mainUICanvas.gameObject.SetActive(true);
    }

    [Command]
    void CmdLeave()
    {
        m_players--;
    }
}
