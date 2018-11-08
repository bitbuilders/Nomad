using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EmoteMessenger : NetworkBehaviour
{
    [SerializeField] List<Emote> m_emotes = null;
    [SerializeField] EmoteCreator m_emoteCreator = null;

    public void Emote(string emote)
    {
        CmdSendEmote(emote);
    }

    [Command]
    void CmdSendEmote(string emote)
    {
        RpcReceiveEmote(emote);
    }

    [ClientRpc]
    void RpcReceiveEmote(string emote)
    {
        Sprite image = GetEmoteImage(emote);
        if (image)
            m_emoteCreator.CreateEmote(image);
    }

    Sprite GetEmoteImage(string emote)
    {
        Sprite image = null;

        foreach (Emote e in m_emotes)
        {
            if (e.Name == emote)
            {
                image = e.Sprite;
                break;
            }
        }

        return image;
    }
}
