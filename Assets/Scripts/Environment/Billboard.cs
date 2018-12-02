using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Billboard : NetworkBehaviour
{
    [SerializeField] GameObject m_billboardGame = null;
    [SerializeField] GameObject m_gameCanvasRoot = null;
    [SerializeField] Canvas m_canvas = null;
    [SerializeField] Canvas m_mainUICanvas;
    [SerializeField] BillboardText m_bbText = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 3.0f;
    [SerializeField] Transform m_cameraPosition = null;
    [SerializeField] LayerMask m_playerMask = 0;

    public Canvas Canvas { get { return m_canvas; } }
    public GameObject CanvasGame { get { return m_gameCanvasRoot; } }
    public BillboardGame.GameName Game { get; set; }

    [SyncVar] public int m_nextPlayer = 0;
    [SyncVar] public int m_players = 0;
    [SyncVar] public SyncListInt m_playerScores = new SyncListInt();
    public BillboardGame m_bbg;

    private void Start()
    {
        GameObject go = Instantiate(m_billboardGame, transform);
        m_bbg = go.GetComponent<BillboardGame>();
        if (m_bbg.GetType() == typeof(SpaceBattle))
            Game = BillboardGame.GameName.SPACE_BATTLE;
        name = Game.ToString();
        m_bbg.Initialize(this);

        BillboardManager.Instance.RegisterBillboard(this);
    }

    private void Update()
    {
        if (Input.GetButtonDown("PlayBillboard") && !m_bbg.Playing)
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
        BillboardGame.PlayerType pt = (BillboardGame.PlayerType)m_nextPlayer;
        m_bbg.StartGame(pt);
        BillboardMessenger bbm = LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>();
        bbm.PlayBillboard(Game);
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<PlayerMovement>().AddState(PlayerMovement.PlayerState.GAME);
        localPlayer.Camera.GetComponent<CameraMovement>().TargetPoint = m_cameraPosition;
        localPlayer.Camera.cullingMask &= ~m_playerMask;
        m_mainUICanvas.gameObject.SetActive(false);
    }

    public void LeaveBillboard()
    {
        m_bbg.StopGame();
        BillboardMessenger bbm = LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>();
        bbm.LeaveBillboard(m_bbg.PlayerNumber);
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<PlayerMovement>().RemoveState(PlayerMovement.PlayerState.GAME);
        m_mainUICanvas.gameObject.SetActive(true);
        localPlayer.Camera.cullingMask |= m_playerMask;
    }

    public void ShowScoreText()
    {
        string text = m_playerScores[0].ToString();
        for (int i = 1; i < m_playerScores.Count; i++)
        {
            text += " | " + m_playerScores[i];
        }
        m_bbText.SetText(text);
        
        m_bbText.BlinkText();
    }

    [Server]
    public void AddScore(BillboardGame.PlayerType pt)
    {
        int index = (int)pt;
        m_playerScores[index] = m_playerScores[index] + 1;
        //print(m_playerScores[index]);
    }

    [Server]
    public void InitializeScores()
    {
        m_playerScores.Clear();
        for (int i = 0; i < m_bbg.MaxPlayers; i++)
        {
            m_playerScores.Add(0);
        }
    }
}
