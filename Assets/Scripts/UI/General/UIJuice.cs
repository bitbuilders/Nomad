using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIJuice : Singleton<UIJuice>
{
    public void FadeAlpha(Image image, bool fadeIn, float time, bool deactivateAfter, float delay = 0.0f)
    {
        StartCoroutine(FadeImage(image, fadeIn, time, deactivateAfter, delay));
    }

    public void FadeAlpha(List<Image> images, bool fadeIn, float time, bool deactivateAfter, float delay = 0.0f)
    {
        StartCoroutine(FadeImage(images, fadeIn, time, deactivateAfter, delay));
    }

    public void FadeImageColor(Image image, Color startColor, Color endColor, float time, bool deactivateAfter, float delay = 0.0f)
    {
        StartCoroutine(FadeIColor(image, startColor, endColor, time, deactivateAfter, delay));
    }

    public void FadeImageColor(List<Image> images, Color startColor, Color endColor, float time, bool deactivateAfter, float delay = 0.0f)
    {
        StartCoroutine(FadeIColor(images, startColor, endColor, time, deactivateAfter, delay));
    }

    public void FadeText(TextMeshProUGUI text, bool fadeIn, float time, bool deactivateAfter, float delay = 0.0f)
    {
        StartCoroutine(FadeTextAlpha(text, fadeIn, time, deactivateAfter, delay));
    }

    public void Blink(TextMeshProUGUI text, float fadeInTime, float fadeOutTime, bool deactivateAfter, float linger= 0.0f)
    {
        SetTextAlpha(text, 0.0f);
        StartCoroutine(BlinkText(text, fadeInTime, fadeOutTime, deactivateAfter, linger));
    }

    public void SetImageAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    IEnumerator FadeImage(Image image, bool fadeIn, float time, bool deactivateAfter, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (fadeIn)
        {
            SetImageAlpha(image, 0.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetImageAlpha(image, i / time);
                yield return null;
            }
            SetImageAlpha(image, 1.0f);
        }
        else
        {
            SetImageAlpha(image, 1.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetImageAlpha(image, 1.0f - (i / time));
                yield return null;
            }
            SetImageAlpha(image, 0.0f);
        }

        if (deactivateAfter)
            image.gameObject.SetActive(false);
    }

    public void SetImageAlpha(List<Image> images, float alpha)
    {
        foreach (Image i in images)
        {
            Color c = i.color;
            c.a = alpha;
            i.color = c;
        }
    }

    IEnumerator FadeImage(List<Image> images, bool fadeIn, float time, bool deactivateAfter, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (fadeIn)
        {
            SetImageAlpha(images, 0.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetImageAlpha(images, i / time);
                yield return null;
            }
            SetImageAlpha(images, 1.0f);
        }
        else
        {
            SetImageAlpha(images, 1.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetImageAlpha(images, 1.0f - (i / time));
                yield return null;
            }
            SetImageAlpha(images, 0.0f);
        }

        if (deactivateAfter)
            foreach (Image i in images) { i.gameObject.SetActive(false); }
    }

    IEnumerator FadeIColor(Image image, Color startColor, Color endColor, float time, bool deactivateAfter, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.color = startColor;
        for (float i = 0.0f; i <= time; i += Time.deltaTime)
        {
            float t = i / time;
            Color c = Color.Lerp(startColor, endColor, t);
            image.color = c;

            yield return null;
        }
        image.color = endColor;

        if (deactivateAfter)
            image.gameObject.SetActive(false);
    }

    public void SetImageColor(List<Image> images, Color color)
    {
        foreach (Image i in images)
        {
            i.color = color;
        }
    }

    IEnumerator FadeIColor(List<Image> images, Color startColor, Color endColor, float time, bool deactivateAfter, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetImageColor(images, startColor);
        for (float i = 0.0f; i <= time; i += Time.deltaTime)
        {
            float t = i / time;
            Color c = Color.Lerp(startColor, endColor, t);
            SetImageColor(images, c);
            
            yield return null;
        }
        SetImageColor(images, endColor);
        if (deactivateAfter)
            foreach (Image i in images) { i.gameObject.SetActive(false); }
    }

    public void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }

    IEnumerator FadeTextAlpha(TextMeshProUGUI text, bool fadeIn, float time, bool deactivateAfter, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (fadeIn)
        {
            SetTextAlpha(text, 0.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetTextAlpha(text, i / time);
                yield return null;
            }
            SetTextAlpha(text, 1.0f);
        }
        else
        {
            SetTextAlpha(text, 1.0f);
            for (float i = 0.0f; i <= time; i += Time.deltaTime)
            {
                SetTextAlpha(text, 1.0f - (i / time));
                yield return null;
            }
            SetTextAlpha(text, 0.0f);
        }

        if (deactivateAfter)
            text.gameObject.SetActive(false);
    }

    IEnumerator BlinkText(TextMeshProUGUI text, float fadeInTime, float fadeOutTime, bool deactivateAfter, float linger)
    {
        FadeText(text, true, fadeInTime, false);
        yield return new WaitForSeconds(fadeInTime + 0.001f + linger);
        FadeText(text, false, fadeOutTime, deactivateAfter);
    }
}
