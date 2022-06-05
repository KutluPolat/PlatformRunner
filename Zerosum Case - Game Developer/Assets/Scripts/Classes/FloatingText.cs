using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingText : ObjectPool
{
    #region Singleton

    public static FloatingText Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion // Singleton

    private const float FLOATING_DURATION = 0.5f;

    public void FloatGoldText(float amount, Vector3 position, bool isSlower)
    {
        Color targetColor = amount >= 0 ? Color.green : Color.red;
        string prefix = amount >= 0 ? "+" : "";

        StartCoroutine(FloatTextCoroutine(prefix + amount.ToString() + "$", position, targetColor, isSlower));
    }

    public void FloatText(string text, Vector3 position, bool isSlower)
    {
        StartCoroutine(FloatTextCoroutine(text, position, Color.green, isSlower));
    }

    private IEnumerator FloatTextCoroutine(string text, Vector3 position, Color color, bool isSlower)
    {
        float duration = isSlower ? FLOATING_DURATION * 3f : FLOATING_DURATION;

        GameObject floatingText = GetObjectFromPool();
        TextMeshPro textMeshPro = floatingText.GetComponentInChildren<TextMeshPro>();

        floatingText.transform.localScale = new Vector3(-1, 1, 1);

        position.y += 2f;
        position += Random.insideUnitSphere;

        Vector3 endPosition = position + Vector3.up * 2f + Vector3.forward * Random.Range(-5f, 5f) + Vector3.right * Random.Range(-5f, 5f);

        floatingText.transform.position = position;

        textMeshPro.color = color;
        textMeshPro.text = text;

        floatingText.transform.DOMove(endPosition, duration * 0.9f);

        textMeshPro.DOFade(1, 0.01f);
        yield return new WaitForSeconds(0.02f);
        textMeshPro.DOFade(0, duration * 0.8f);

        yield return new WaitForSeconds(duration);
        SendObjectBackToPool(floatingText);
    }
}
