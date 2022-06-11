using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zerosum.PlatformRunner.Enums;

public class AnimationHandler : MonoBehaviour, IEvents
{
    #region Variables

    public AnimState CurrentAnimState { get; private set; }
    [SerializeField, Min(0)] private float _animationBlendDuration = 0.5f;
    [SerializeField] private Movement _movement;
    [SerializeField, SceneObjectsOnly] private Animator _rotAnim, _movAnim;

    private int _maxNumOfStack = 1, _currentNumOfStack;
    private float _currentIdleAction, _currentAction;

    private bool IsRunning
    {
        get
        {
            return _movAnim.GetBool("IsRunning");
        }
        set
        {
            _movAnim.SetBool("IsRunning", value);
        }
    }

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
        UpdateMovAnimParameters();
    }

    #endregion // Updates

    #region Methods

    private void HandleRotAnim() => _rotAnim.SetFloat("AvgInput", _movement.GetAverageOfLatestInputs());

    private void HandleMovAnim() => _movAnim.SetFloat("Run", _currentNumOfStack / (float)_maxNumOfStack);

    private void HandleActionAnim(AnimState newAnimState)
    {
        switch (newAnimState)
        {
            case AnimState.Idle:

                CurrentAnimState = AnimState.Idle;
                IsRunning = false;
                DOValues(0, 0);

                break;

            case AnimState.Dance:

                CurrentAnimState = AnimState.Dance;
                IsRunning = false;
                DOValues(1, 0);

                break;

            case AnimState.Run:

                CurrentAnimState = AnimState.Run;
                IsRunning = true;

                break;

            case AnimState.Stumble:

                CurrentAnimState = AnimState.Stumble;
                IsRunning = false;
                DOValues(2, 1);

                break;

            default:
                break;
        } 
    }

    private void DOValues(float endIdleAction, float endAction)
    {
        DOTween.To(() => _currentIdleAction, x => _currentIdleAction = x, endIdleAction, _animationBlendDuration);
        DOTween.To(() => _currentAction, x => _currentAction = x, endAction, _animationBlendDuration);
    }

    private void UpdateMovAnimParameters()
    {
        _movAnim.SetFloat("IdleAction", _currentIdleAction);
        _movAnim.SetFloat("Action", _currentAction);
    }

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
        EventManager.Instance.StateInGame += () => { IsRunning = true; };
        EventManager.Instance.PlayerTrapped += (value) => HandleActionAnim(AnimState.Stumble);
        EventManager.Instance.MovementUnblocked += () => HandleActionAnim(AnimState.Run);
        EventManager.Instance.StateLevelSuccess += () => HandleActionAnim(AnimState.Dance);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StackUpdated -= UpdateParameters;
        EventManager.Instance.StateInGame -= () => { IsRunning = true; };
        EventManager.Instance.PlayerTrapped -= (value) => HandleActionAnim(AnimState.Stumble);
        EventManager.Instance.MovementUnblocked -= () => HandleActionAnim(AnimState.Run);
        EventManager.Instance.StateLevelSuccess -= () => HandleActionAnim(AnimState.Dance);
    }

    #endregion // Events
}
