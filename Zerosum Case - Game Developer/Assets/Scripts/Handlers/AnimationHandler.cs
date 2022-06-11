using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField] private Movement _movement;
    [SerializeField, SceneObjectsOnly] private Animator _rotAnim, _movAnim;

    private int _maxNumOfStack = 1, _currentNumOfStack;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Updates

    private void LateUpdate()
    {
        HandleRotAnim();
        HandleMovAnim();
    }

    #endregion // Updates

    #region Methods

    private void HandleRotAnim() => _rotAnim.SetFloat("AvgInput", _movement.GetAverageOfLatestInputs());

    private void HandleMovAnim() => _movAnim.SetFloat("Run", _currentNumOfStack / (float)_maxNumOfStack);

    private void UpdateParameters()
    {
        _maxNumOfStack = SaveSystem.Instance.MaxNumOfStack;
        _currentNumOfStack = SaveSystem.Instance.CurrentNumOfStack;
    }

    #endregion // Methods

    #region Events

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

    #endregion // Events
}
