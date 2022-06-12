using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingMoneyController : ObjectPool
{
    [SerializeField] private Transform _piggyBank;
    [SerializeField, MinMaxSlider(0, 5)] private Vector2 _radius = new Vector2(1, 3), _firstPhaseScale = new Vector2(1.5f, 2.5f);
    [SerializeField, Min(0.2f)] private float _floatDuration = 1f;
    [SerializeField, Min(3), InfoBox("Higher generate rate means less floating money.")] private float _generateRate = 4f;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameManager.Instance.PlayerTransform;
    }


    public void FloatMoney(float amount)
    {
        StartCoroutine(FloatCoroutine(Mathf.FloorToInt(amount / _generateRate)));
    }

    private IEnumerator FloatCoroutine(int num)
    {
        if(num > 0)
        {
            float durationPerPhase = _floatDuration / 2f;
            List<GameObject> generatedFloatingMoneys = new List<GameObject>();
            float targetDurationToGenerate = (durationPerPhase) / num;
            GameManager.Instance.StackManager.DestroyStackablesInSeconds(durationPerPhase);

            for (int i = 0; i < num; i++)
            {
                generatedFloatingMoneys.Add(GetObjectFromPool());
                generatedFloatingMoneys[i].transform.localScale = Vector3.zero;
                generatedFloatingMoneys[i].transform.position = _playerTransform.position;
                generatedFloatingMoneys[i].transform.DOMove(GetRandomVisiblePosition(), targetDurationToGenerate).SetEase(Ease.OutBack);
                generatedFloatingMoneys[i].transform.DOScale(Random.Range(_firstPhaseScale.x, _firstPhaseScale.y), targetDurationToGenerate).SetEase(Ease.OutBack);
                GameManager.Instance.HapticHandler.MediumHaptic();

                yield return new WaitForSeconds(targetDurationToGenerate);
            }

            yield return new WaitForSeconds(durationPerPhase);

            foreach(GameObject floatingMoney in generatedFloatingMoneys)
            {
                floatingMoney.transform.DOScale(0, durationPerPhase).SetEase(Ease.OutBack);
                floatingMoney.transform.DOMove(_piggyBank.position, durationPerPhase).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    DotweenExtensions.PunchScale(_piggyBank, Ease.OutBack, 1.3f, 1f, 0.2f);
                });
            }
        }
    }

    private Vector3 GetRandomVisiblePosition()
    {
        Vector3 randPos = _playerTransform.position + Random.insideUnitSphere * Random.Range(_radius.x, _radius.y);
        randPos.y = Mathf.Abs(randPos.y);

        return randPos;
    }
}
