using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJuice : Singleton<UIJuice>
{
    public void FadeAlpha(Image image, bool fadeIn, float time)
    {
        StartCoroutine(FadeImage(image, fadeIn, time));
    }

    public void FadeAlpha(List<Image> images, bool fadeIn, float time)
    {
        StartCoroutine(FadeImage(images, fadeIn, time));
    }

    public void FadeColor(Image image, Color startColor, Color endColor, float time)
    {
        StartCoroutine(FadeImageColor(image, startColor, endColor, time));
    }

    public void FadeColor(List<Image> images, Color startColor, Color endColor, float time)
    {
        StartCoroutine(FadeImageColor(images, startColor, endColor, time));
    }

    void SetImageAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    IEnumerator FadeImage(Image image, bool fadeIn, float time)
    {
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
    }

    void SetImageAlpha(List<Image> images, float alpha)
    {
        foreach (Image i in images)
        {
            Color c = i.color;
            c.a = alpha;
            i.color = c;
        }
    }

    IEnumerator FadeImage(List<Image> images, bool fadeIn, float time)
    {
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
    }

    IEnumerator FadeImageColor(Image image, Color startColor, Color endColor, float time)
    {
        image.color = startColor;
        for (float i = 0.0f; i <= time; i += Time.deltaTime)
        {
            float t = i / time;
            Color c = Color.Lerp(startColor, endColor, t);
            image.color = c;

            yield return null;
        }
        image.color = endColor;
    }

    void SetImageColor(List<Image> images, Color color)
    {
        foreach (Image i in images)
        {
            i.color = color;
        }
    }

    IEnumerator FadeImageColor(List<Image> images, Color startColor, Color endColor, float time)
    {
        SetImageColor(images, startColor);
        for (float i = 0.0f; i <= time; i += Time.deltaTime)
        {
            float t = i / time;
            Color c = Color.Lerp(startColor, endColor, t);
            SetImageColor(images, c);
            
            yield return null;
        }
        SetImageColor(images, endColor);
    }
}
