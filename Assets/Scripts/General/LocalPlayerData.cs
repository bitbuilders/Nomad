using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerData : MonoBehaviour
{
    public enum HairType
    {
        DEFAULT,

    }

    public enum BodyType
    {
        DEFAULT,

    }

    public enum EyeType
    {
        DEFAULT,

    }

    public enum GlassesType
    {
        DEFAULT,

    }

    public struct ModelData
    {
        public int ModelNumber;
        public int RowNumber;
        public float HairHue;
        public Vector2 HairValue;
        public float GlassesHue;
        public Vector2 GlassesValue;
        public float WeightValue;
        public float HeightValue;
    }

    public struct PlayerAttributes
    {
        public string Username;
        public string Color;
        public ModelSelector.CharacterAttributes Attributes;
        public ModelData MData;
    }

    public PlayerAttributes Attributes;
    public Player LocalPlayer { get; set; }
    public bool FirstLoadIn { get { return string.IsNullOrEmpty(Attributes.Username); } }
    //public string TempUsername { get; set; }
    //public string TempColor { get; set; }

    public static LocalPlayerData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetAttributes(string username, string color, ModelSelector.CharacterAttributes attributes, ModelData modelData)
    {
        Attributes = new PlayerAttributes() { Username = username, Color = color, Attributes = attributes, MData = modelData };
    }

    public void Initialize(Player localPlayer)
    {
        LocalPlayer = localPlayer;
        LocalPlayer.UserName = Attributes.Username;
    }

    public bool PlayerExists(string playerName)
    {
        bool exists = false;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.UserName == playerName && p != LocalPlayer)
            {
                exists = true;
                break;
            }
        }


        return exists;
    }

    public bool MultipleUsernames()
    {
        bool multiple = false;
        string localUser = LocalPlayer.UserName;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.UserName == localUser)
            {
                multiple = true;
                break;
            }
        }

        return multiple;
    }

    public Player FindPlayerWithID(int id)
    {
        Player player = null;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.ID == id)
            {
                player = p;
                break;
            }
        }

        return player;
    }
}
