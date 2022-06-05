using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EventManager : MonoBehaviour
{
    #region Singleton

    public static EventManager Instance;

    private void Awake()
    {
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

    #region Variables

    [BoxGroup("Is Event Debugs On"), SerializeField]
    private bool _state, _button, _movement;

    #endregion // Variables

    #region Delegates

    public delegate void State();
    public delegate void Button();
    public delegate void Movement();

    #endregion // Delegates

    #region Events

    public event State StateTapToPlay, StateInGame, StateEndingSequence, StateLevelSuccess, StateLevelFailed, StateLevelEnd;
    public event Button PressedNextLevel, PressedRestart;
    public event Movement MovementBlocked, MovementUnblocked;

    #endregion // Events

    #region Methods

    #region Movement

    public void OnMovementUnblocked()
    {
        EventTrigger(MovementUnblocked, "OnMovementUnblocked");
    }

    public void OnMovementBlocked()
    {
        EventTrigger(MovementBlocked, "OnMovementBlocked");
    }

    #endregion // Movement

    #region Button

    public void OnPressedNextLevel()
    {
        EventTrigger(PressedNextLevel, "OnPressedNextLevel");
    }

    public void OnPressedRestart()
    {
        EventTrigger(PressedRestart, "OnPressedRestart");
    }

    #endregion // Button

    #region State  

    public void OnStateLevelSuccess()
    {
        GameManager.Instance.IsSuccess = true;
        EventTrigger(StateLevelSuccess, "OnStateLevelSuccess");
        OnStateLevelEnd();
    }

    public void OnStateLevelFailed()
    {
        GameManager.Instance.IsSuccess = false;
        EventTrigger(StateLevelFailed, "OnStateLevelFailed");
        OnStateLevelEnd();
    }

    public void OnStateTapToPlay()
    {
        GameManager.Instance.IsSuccess = false;
        EventTrigger(StateTapToPlay, "OnStateTapToPlay");
    }

    public void OnStateInGame()
    {
        EventTrigger(StateInGame, "OnStateInGame");
    }

    public void OnStateEndingSequence()
    {
        EventTrigger(StateEndingSequence, "OnStateEndingSequence");
    }

    private void OnStateLevelEnd()
    {
        EventTrigger(StateLevelEnd, "OnStateLevelEnd");
    }

    #endregion // State

    #region EventTriggers

    private void EventTrigger(Movement movementEvent, string methodName)
    {
        if (movementEvent != null)
        {
            LogIfActive(_movement, methodName);
            movementEvent();
        }
    }

    private void EventTrigger(State stateEvent, string methodName)
    {
        if (stateEvent != null)
        {
            LogIfActive(_state, methodName);
            stateEvent();
        }
    }

    private void EventTrigger(Button buttonEvent, string methodName)
    {
        if (buttonEvent != null)
        {
            LogIfActive(_button, methodName);
            buttonEvent();
        }
    }

    private void LogIfActive(bool isDebugActive, string methodName)
    {
#if UNITY_EDITOR
        if (isDebugActive)
        {
            Debug.Log($"{methodName} triggered.");
        }
#endif
    }

    #endregion // EventTriggers
    #endregion // Methods
}
