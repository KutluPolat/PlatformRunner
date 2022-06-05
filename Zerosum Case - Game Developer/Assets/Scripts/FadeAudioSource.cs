using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class FadeAudioSource
{
    public static void StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        DOTween.To(() => audioSource.volume, volume => audioSource.volume = volume, targetVolume, duration);
    }
}