using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessageRoom : MonoBehaviour
{
    public string Messages { get; set; }
    public string Name { get; set; }

    private void Start()
    {
        Messages = Colors.ConvertToColor("This is a private room with " + Name, Colors.ColorType.WHITE);
    }

    public void SetAsCurrentRoom()
    {
        DirectMessageManager.Instance.SetCurrentMessageRoom(this);
    }
}
