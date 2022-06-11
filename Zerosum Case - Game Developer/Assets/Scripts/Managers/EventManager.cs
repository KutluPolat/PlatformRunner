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
    private bool _state, _button, _movement, _stack, _exchange, _gold, _trap;

    #endregion // Variables

    #region Delegates

    public delegate void State();
    public delegate void Button();
    public delegate void Movement();
    public delegate void StackableDelegate(StackableController stackable);
    public delegate void ExchangeDelegate(StackableController exchangedStackable);
    public delegate void GoldDelegate();
    public delegate void TrapDelegate(StackableController trappedStackable);

    #endregion // Delegates

    #region Events

    public event State StateTapToPlay, StateInGame, StateEndingSequence, StateLevelSuccess, StateLevelFailed, StateLevelEnd;
    public event Button PressedNextLevel, PressedRestart;
    public event Movement MovementBlocked, MovementUnblocked;
    public event StackableDelegate StackCollected;
    public event ExchangeDelegate StackableExchanged;
    public event GoldDelegate GoldCollected, GoldUpdated, GoldLost;
    public event TrapDelegate PlayerTrapped, StackableTrapped;

    #endregion // Events

    #region Methods

    #region Trap

    public void OnStackableTrapped(StackableController trappedStackable)
    {
        EventTrigger(trappedStackable, StackableTrapped, "OnStackableTrapped");
    }

    public void OnPlayerTrapped(StackableController trappedStackable)
    {
        EventTrigger(trappedStackable, PlayerTrapped, "OnPlayerTrapped");
    }

    #endregion // Trap

    #region Gold

    private void OnGoldUpdated()
    {
        EventTrigger(GoldUpdated, "OnGoldUpdated");
    }

    public void OnGoldLost()
    {
        EventTrigger(GoldLost, "OnGoldLost");
        OnGoldUpdated();
    }

    public void OnGoldCollected()
    {
        EventTrigger(GoldCollected, "OnGoldCollected");
        OnGoldUpdated();
    }

    #endregion // Gold

    #region Exchange

    public void OnStackableExchanged(StackableController exchangedStackable)
    {
        EventTrigger(exchangedStackable, StackableExchanged, "OnStackableExchanged");
    }

    #endregion // Exchange

    #region Stacks

    public void OnStackCollected(StackableController stack)
    {
        EventTrigger(stack, StackCollected, "OnStackCollected");
    }

    #endregion // Stacks

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
    }

    public void OnStateLevelFailed()
    {
        GameManager.Instance.IsSuccess = false;
        EventTrigger(StateLevelFailed, "OnStateLevelFailed");
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

    public void OnStateLevelEnd()
    {
        EventTrigger(StateLevelEnd, "OnStateLevelEnd");
    }

    #endregion // State

    #region EventTriggers

    private void EventTrigger(StackableController trappedStackable, TrapDelegate trapEvent, string methodName)
    {
        if (trapEvent != null)
        {
            LogIfActive(_trap, methodName);
            trapEvent(trappedStackable);
        }
    }

    private void EventTrigger(GoldDelegate goldEvent, string methodName)
    {
        if(goldEvent != null)
        {
            LogIfActive(_gold, methodName);
            goldEvent();
        }
    }

    private void EventTrigger(StackableController exchangedStackable, ExchangeDelegate exchangeEvent, string methodName)
    {
        if(exchangeEvent != null)
        {
            LogIfActive(_exchange, methodName);
            exchangeEvent(exchangedStackable);
        }
    }

    private void EventTrigger(StackableController stackable, StackableDelegate stackableEvent, string methodName)
    {
        if(stackableEvent != null)
        {
            LogIfActive(_stack, methodName);
            stackableEvent(stackable);
        }
    }

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
