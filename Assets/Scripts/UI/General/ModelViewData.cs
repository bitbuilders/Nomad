using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelViewData : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer m_hairRenderer = null;
    [SerializeField] SkinnedMeshRenderer m_glassesRenderer = null;

    public Material HairMaterial;
    public Material GlassesMaterial;

    private void Awake()
    {
        Material hairMat = Instantiate(m_hairRenderer.material, transform);
        m_hairRenderer.material = hairMat;
        Material glassesMat = Instantiate(m_glassesRenderer.material, transform);
        m_glassesRenderer.material = glassesMat;
        HairMaterial = hairMat;
        GlassesMaterial = glassesMat;
    }
}
