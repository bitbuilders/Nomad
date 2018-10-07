using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T ms_instance = null;

    public static T Instance
    {
        get
        {
            if (ms_instance == null)
            {
                ms_instance = FindObjectOfType<T>();

                if (ms_instance == null)
                {
                    GameObject go = new GameObject("Singleton " + typeof(T));
                    T t = go.AddComponent<T>();
                    ms_instance = t;
                }
            }

            return ms_instance;
        }
    }
}
