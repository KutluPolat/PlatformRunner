using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlendHandler : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField]
    private Animator _cameraAnimator;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Methods

    private void ChangeCameraStateTo(string stateName) => _cameraAnimator.Play(stateName);

    #endregion // Methods

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay += () => ChangeCameraStateTo("TapToPlay");
        EventManager.Instance.StateEndingSequence += () => ChangeCameraStateTo("EndingSequence");
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay -= () => ChangeCameraStateTo("TapToPlay");
        EventManager.Instance.StateEndingSequence -= () => ChangeCameraStateTo("EndingSequence");
    }

    #endregion // Events
}
