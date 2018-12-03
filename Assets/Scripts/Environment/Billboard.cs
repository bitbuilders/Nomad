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
    [SerializeField] TextMeshProUGUI m_playerCountText = null;
    [SerializeField] TextMeshProUGUI m_playText1 = null;
    [SerializeField] TextMeshProUGUI m_playText2 = null;
    [SerializeField] BillboardText m_bbText = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 3.0f;
    [SerializeField] Transform m_interactionPoint = null;
    [SerializeField] Transform m_cameraPosition = null;
    [SerializeField] LayerMask m_playerMask = 0;
    [SerializeField] [Range(0.0f, 5.0f)] float m_missingPlayerCheck = 2.0f;

    public Canvas Canvas { get { return m_canvas; } }
    public GameObject CanvasGame { get { return m_gameCanvasRoot; } }
    public BillboardGame.GameName Game { get; set; }

    [SyncVar] public int m_nextPlayer = 0;
    [SyncVar] public int m_players = 0;
    public SyncListInt m_playerScores = new SyncListInt();
    public SyncListInt m_playerIDs = new SyncListInt();
    public BillboardGame m_bbg;
    float m_missingTime;
    float m_fadeTime;

    private void Start()
    {
        GameObject go = Instantiate(m_billboardGame, CanvasGame.transform);
        m_bbg = go.GetComponent<BillboardGame>();
        if (m_bbg.GetType() == typeof(SpaceBattle))
            Game = BillboardGame.GameName.SPACE_BATTLE;
        name = Game.ToString();
        m_bbg.Initialize(this);

        BillboardManager.Instance.RegisterBillboard(this);
        //OnPlayerCountChange(m_players);
    }

    private void Update()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        Vector3 dir = localPlayer.transform.position - m_interactionPoint.position;
        if (dir.magnitude <= m_interactionDistance && !m_bbg.Playing)
        {
            m_fadeTime += Time.deltaTime;
            m_fadeTime = Mathf.Clamp(m_fadeTime, 0.0f, 0.5f);
            float a = m_fadeTime / 0.5f;
            UIJuice.Instance.SetTextAlpha(m_playText1, a);
            UIJuice.Instance.SetTextAlpha(m_playText2, a);
        }
        else
        {
            m_fadeTime -= Time.deltaTime;
            m_fadeTime = Mathf.Clamp(m_fadeTime, 0.0f, 0.5f);
            float a = m_fadeTime / 0.5f;
            UIJuice.Instance.SetTextAlpha(m_playText1, a);
            UIJuice.Instance.SetTextAlpha(m_playText2, a);
        }

        if (Input.GetButtonDown("PlayBillboard") && !m_bbg.Playing)
        {
            if (dir.magnitude <= m_interactionDistance)
            {
                if (m_players < m_bbg.MaxPlayers)
                {
                    PlayBillboard(localPlayer.ID);
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

        m_missingTime += Time.deltaTime;
        if (m_missingTime >= m_missingPlayerCheck)
        {
            m_missingTime = 0.0f;
            for (int i = 0; i < m_playerIDs.Count; i++)
            {
                int id = m_playerIDs[i];
                if (id != 0)
                {
                    Player p = LocalPlayerData.Instance.FindPlayerWithID(id);
                    if (p == null)
                    {
                        LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>().LeaveBillboard((BillboardGame.PlayerType)i);
                        m_playerIDs[i] = 0;
                        print("removed inactive player");
                    }
                }
            }
        }

        int max = m_bbg.MaxPlayers == 0 ? 2 : m_bbg.MaxPlayers;
        string message = m_players + " / " + max + " Players";
        m_playerCountText.text = message;
    }

    //void OnPlayerCountChange(int currentPlayers)
    //{

    //}

    public void PlayBillboard(int id)
    {
        if (m_nextPlayer >= m_bbg.MaxPlayers)
            return;

        BillboardGame.PlayerType pt = (BillboardGame.PlayerType)m_nextPlayer;
        m_bbg.StartGame(pt);
        BillboardMessenger bbm = LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>();
        bbm.PlayBillboard(Game, pt, id);
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
        m_playerIDs.Clear();
        for (int i = 0; i < m_bbg.MaxPlayers; i++)
        {
            m_playerScores.Add(0);
            m_playerIDs.Add(0);
        }
    }

    [Server]
    public void AddPlayer(BillboardGame.PlayerType pt, int playerID)
    {
        m_players++;
        int player = (int)pt;
        m_playerIDs[player] = playerID;
    }

    [Server]
    public void RemovePlayer(BillboardGame.PlayerType pt)
    {
        m_players--;
        int player = (int)pt;
        m_playerIDs[player] = 0;
    }

    [Server]
    public void SetNextPlayer()
    {
        m_nextPlayer = m_bbg.MaxPlayers;
        for (int i = 0; i < m_playerIDs.Count; i++)
        {
            if (m_playerIDs[i] == 0)
            {
                m_nextPlayer = i;
                break;
            }
        }
    }
}
