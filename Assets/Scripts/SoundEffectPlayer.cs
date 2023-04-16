using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public enum Sfx
    {
        Walk,
        Jump,
        Fall,
        StompLanding,
        Hit,
        Death,
        NewItem
    }

    public static SoundEffectPlayer instance;
    public List<AudioClip> soundEffects;
    public AudioSource Source;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySfx(Sfx sfx)
    {
        StopAllCoroutines();
        Source.clip = soundEffects[(int)sfx];
        Source.PlayOneShot(Source.clip);
    }

    public void PlayWalkSfx(float f)
    {
        StopAllCoroutines();
        StartCoroutine(PlayWalkSfxCor(f));
    }

    private IEnumerator PlayWalkSfxCor(float f)
    {
        var wait = new WaitForEndOfFrame();
        float t = 0;

        while (true)
        {
            Source.clip = soundEffects[(int)Sfx.Walk];
            Source.PlayOneShot(Source.clip);
            
            while (t < f)
            {
                t += Time.deltaTime;
                yield return wait;
            }

            t = 0;
            
            yield return wait;
        }
    }

    public void KillSounds()
    {
        StopAllCoroutines();
    }
}
