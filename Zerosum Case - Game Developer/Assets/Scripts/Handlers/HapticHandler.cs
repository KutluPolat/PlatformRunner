using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticHandler : MonoBehaviour, IEvents
{
    #region Variables

    private readonly float _hapticMinimumDelay = 0.05f;
    private float _timer;

    private delegate void TapticDelegate();
    private event TapticDelegate _tapticMethod;

    #endregion // Variables

    #region Start

    private void Start()
    {
        MediumHaptic();
        SubscribeEvents();
    }

    #endregion // Start

    #region Update

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    #endregion // Update

    #region HapticMethods

    private void MediumHaptic()
    {
        _tapticMethod = Taptic.Medium;
        ApplyTaptic();
    }

    private void SuccessHaptic()
    {
        _tapticMethod = Taptic.Success;
        ApplyTaptic();
    }

    private void FailureHaptic()
    {
        _tapticMethod = Taptic.Failure;
        ApplyTaptic();
    }

    private void ApplyTaptic()
    {
        if(_timer <= 0)
        {
            _timer += _hapticMinimumDelay;
            _tapticMethod();
        }
    }

    #endregion // HapticMethods

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.StateLevelSuccess += SuccessHaptic;
        EventManager.Instance.StateLevelFailed += FailureHaptic;
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StateLevelSuccess -= SuccessHaptic;
        EventManager.Instance.StateLevelFailed -= FailureHaptic;
    }

    #endregion // Events
}
