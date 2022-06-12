using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class FilledBarHandler : MonoBehaviour, IEvents
{
    [SerializeField] private ProceduralImage _filledBar;
    private int _maxNumOfStack = 1, _currentNumOfStack;

    private void Start()
    {
        SubscribeEvents();
    }

    private void LateUpdate()
    {
        UpdateFilledBar();
    }

    private void UpdateFilledBar()
    {
        _filledBar.fillAmount = _currentNumOfStack / (float)_maxNumOfStack;
    }

    private void UpdateParameters()
    {
        _maxNumOfStack = SaveSystem.Instance.MaxNumOfStack;
        _currentNumOfStack = SaveSystem.Instance.CurrentNumOfStack;
    }

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.StackUpdated += UpdateParameters;
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StackUpdated -= UpdateParameters;
    }
}
