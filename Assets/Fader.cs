using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader instance;
    public Image faderImage;

    private void Awake()
    {
        instance = this;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        var wait = new WaitForEndOfFrame();
        float t = 0;
        float dur = 2f;
        float percentage;
        var color = faderImage.color;

        while (t < dur)
        {
            t += Time.deltaTime;
            percentage = t / dur;
            color.a = 1 - percentage;
            faderImage.color = color;
            yield return wait;
        }
    }
    
    public void FadeOut(float fadeAmount)
    {
        var faderImageColor = faderImage.color;
        faderImageColor.a = fadeAmount;
        faderImage.color = faderImageColor;
    }
}
