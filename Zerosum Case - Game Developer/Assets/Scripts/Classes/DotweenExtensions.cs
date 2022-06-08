using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenExtensions
{
    public delegate void Activity();

    /// <summary>
    /// It's safer than normal DoPunchScale. Because it will always keep the original scale on repeated inputs.
    /// </summary>
    /// <param name="transform">Transform which dotween will be applied.</param>
    /// <param name="targetScale">Target scale of transform</param>
    /// <param name="originalScale">Original scale of transform</param>
    /// <param name="duration">Total duraion of tween</param>
    public static void PunchScale(Transform transform, float targetScale = 1.2f, float originalScale = 1f, float duration = 0.5f)
    {
        transform.DOScale(originalScale, duration * 0.1f).OnComplete(() =>
        {
            transform.DOScale(targetScale, duration * 0.45f).OnComplete(() =>
            {
                transform.DOScale(originalScale, duration * 0.45f);
            });
        });
    }

    public static void PunchRotation(Transform transform, Vector3 originalRotation,  Vector3 rotationPower, float duration = 0.4f)
    {
        transform.DORotate(originalRotation + rotationPower, duration * 0.1f).OnComplete(() =>
        {
            transform.DORotate(originalRotation + rotationPower, duration * 0.45f).OnComplete(() =>
            {
                transform.DORotate(originalRotation, duration * 0.45f);
            });
        });
    }

    public static void Delay(Activity delayedActivity, float duration)
    {
        DOTween.To(() => 0, x => x = 0, 0, duration).OnComplete(() => { delayedActivity(); }); 
    }
}
