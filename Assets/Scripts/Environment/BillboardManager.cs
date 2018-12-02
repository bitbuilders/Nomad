using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BillboardManager : Singleton<BillboardManager>
{
    List<Billboard> m_billboards;

    private void Awake()
    {
        m_billboards = new List<Billboard>();
    }

    public void RegisterBillboard(Billboard bb)
    {
        m_billboards.Add(bb);
    }

    public Billboard GetBillboard(BillboardGame.GameName game)
    {
        Billboard bb = null;

        foreach (Billboard b in m_billboards)
        {
            if (b.Game == game)
            {
                bb = b;
                break;
            }
        }

        return bb;
    }
}
