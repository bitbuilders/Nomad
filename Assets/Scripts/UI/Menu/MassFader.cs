using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MassFader : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> m_textToFade = null;
    [SerializeField] List<Image> m_imagesToFade = null;
    [SerializeField] bool m_startFadedOut = false;

    private void Start()
    {
        if (m_startFadedOut)
        {
            UIJuice.Instance.SetImageAlpha(m_imagesToFade, 0.0f);
            UIJuice.Instance.SetTextAlpha(m_textToFade, 0.0f);
        }
    }

    public void FadeIn()
    {
        UIJuice.Instance.FadeAlpha(m_imagesToFade, 1.0f, true, 0.2f, false);
        UIJuice.Instance.FadeText(m_textToFade, 1.0f, true, 0.2f, false);
    }

    public void FadeOut(bool deactivateAllAfter, bool deactivateParentAfter)
    {
        float time = 0.2f;
        UIJuice.Instance.FadeAlpha(m_imagesToFade, 0.0f, false, time, deactivateAllAfter);
        UIJuice.Instance.FadeText(m_textToFade, 0.0f, false, time, deactivateAllAfter);

        StartCoroutine(DeactivateParent(time));
    }

    IEnumerator DeactivateParent(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
