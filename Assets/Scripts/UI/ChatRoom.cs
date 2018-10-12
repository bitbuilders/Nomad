using UnityEngine;
using TMPro;
using System.Text;

public class ChatRoom : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_chatLog = null;
    [SerializeField] TextMeshProUGUI m_roomNameText = null;
    [SerializeField] GameObject m_chatTextContainer = null;
    //[SerializeField] [Range(0.0f, 10.0f)] float m_slideSpeed = 1.0f;
    
    public Player LocalOwner { get; private set; }
    public int ID { get; private set; }
    public string Name { get; private set; }

    StringBuilder m_text;
    RectTransform m_containerBounds;
    float m_targetHeight;
    float m_currentHeight;
    float m_time;

    void Start()
    {
        m_text = new StringBuilder(m_chatLog.text);
        m_containerBounds = m_chatTextContainer.GetComponent<RectTransform>();
        AddMessage("Hi there");
    }

    public void Initialize(Player localOwner, int roomID)
    {
        LocalOwner = localOwner;

        if (roomID <= 0)
            ID = ChatRoomAssigner.Instance.GetRoomID();
        else
            ID = roomID;
    }

    public void SetRoomName(string roomName)
    {
        m_roomNameText.text = roomName;
    }

    void Update()
    {
        //if (m_time <= 1.0f)
        //{
        //    m_time += Time.deltaTime * m_slideSpeed;
        //    float t = Interpolation.BounceOut(m_time);
        //    float size = Mathf.Lerp(m_currentHeight, m_targetHeight, t);
        //    m_containerBounds.sizeDelta = new Vector2(m_containerBounds.sizeDelta.x, size);
        //}
    }

    public void AddMessage(string text)
    {
        if (m_text.Length > 0)
        {
            m_text.Append("\n");
            m_currentHeight = m_containerBounds.sizeDelta.y;
        }
        else
        {
            m_currentHeight = 0.0f;
        }

        m_text.Append(text);
        m_chatLog.text = m_text.ToString();
        
        Vector2 parentSize = GetComponentInParent<RectTransform>().GetComponentInParent<RectTransform>().sizeDelta;
        float padding = m_chatLog.margin.x * 2.0f;
        Vector2 size = m_chatLog.GetPreferredValues(m_chatLog.text, parentSize.x - padding, Mathf.Infinity);
        m_containerBounds.sizeDelta = new Vector2(m_containerBounds.sizeDelta.x, size.y);
        m_targetHeight = size.y;
        m_time = 0.0f;
    }
}
