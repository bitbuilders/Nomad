using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] BillBoardGame m_billboardGame = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_interactionDistance = 3.0f;

    private void Update()
    {
        if (Input.GetButtonDown("PlayBillboard"))
        {
            PlayBillboard();
        }
    }

    public void PlayBillboard()
    {

    }
}
