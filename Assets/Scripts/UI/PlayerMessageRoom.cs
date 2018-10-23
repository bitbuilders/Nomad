using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMessageRoom : MonoBehaviour
{
    public string Messages { get; set; }
    public string Name { get; set; }

    DirectMessageInterface m_messageInterface;

    private void Start()
    {
        Initialize();
    }

    // Will sometimes need to call before start
    public void Initialize()
    {
        Messages = Colors.ConvertToColor("This is a private room with " + Name, Colors.ColorType.WHITE) + Messages;
        m_messageInterface = GameObject.Find("Menu").GetComponent<DirectMessageInterface>();
    }

    public void SetAsCurrentRoom()
    {
        DirectMessageManager.Instance.SetCurrentMessageRoom(this);
        m_messageInterface.InitializeInputField();
    }

    public void AddMessage(string message)
    {
        StringBuilder sb = new StringBuilder(Messages);
        sb.Append("\n");
        sb.Append(message);

        Messages = sb.ToString();
    }
}
