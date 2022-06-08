using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Zerosum.PlatformRunner.Enums;

public class GameManager : MonoBehaviour, IEvents
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
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

    public const int NumOfStackableType = 3;
    public float FeedbackLimitDuration = 0.05f;
    public bool IsSuccess { get; set; }
    private GameState _currentGameState;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region StateControls

    public void SetGameStateTo(GameState newState) => _currentGameState = newState;
    public bool IsGameStateEqualsTo(GameState thisState)
    {
        return thisState == _currentGameState;
    }
    public bool IsGameStateEqualsTo(ComparerType comparer, params GameState[] thisState)
    {
        bool isGameStateEqualsOneOfThem = false;

        foreach (GameState gameState in thisState)
        {
            switch (comparer)
            {
                case ComparerType.Or:
                    isGameStateEqualsOneOfThem |= gameState == _currentGameState;
                    break;

                case ComparerType.And:
                    isGameStateEqualsOneOfThem &= gameState == _currentGameState;
                    break;
            }
        }

        return isGameStateEqualsOneOfThem;
    }
    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

    #endregion // StateControls

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay += () => SetGameStateTo(GameState.TapToPlay);
        EventManager.Instance.StateInGame += () => SetGameStateTo(GameState.InGame);
        EventManager.Instance.StateEndingSequence += () => SetGameStateTo(GameState.EndingSequence);
        EventManager.Instance.StateLevelSuccess += () => SetGameStateTo(GameState.LevelSuccess);
        EventManager.Instance.StateLevelFailed += () => SetGameStateTo(GameState.LevelFailed);
        EventManager.Instance.StateLevelEnd += () => SetGameStateTo(GameState.LevelEnd);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay -= () => SetGameStateTo(GameState.TapToPlay);
        EventManager.Instance.StateInGame -= () => SetGameStateTo(GameState.InGame);
        EventManager.Instance.StateEndingSequence -= () => SetGameStateTo(GameState.EndingSequence);
        EventManager.Instance.StateLevelSuccess -= () => SetGameStateTo(GameState.LevelSuccess);
        EventManager.Instance.StateLevelFailed -= () => SetGameStateTo(GameState.LevelFailed);
        EventManager.Instance.StateLevelEnd -= () => SetGameStateTo(GameState.LevelEnd);
    }

    #endregion // Events
}
