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
            InitializeEdges();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion // Singleton

    #region Variables

    public Transform PlayerTransform;
    [SerializeField, FoldoutGroup("Platform")] private MeshRenderer _platformMesh;
    [SerializeField, Min(0), FoldoutGroup("Platform")] private float _safeEdgeDistance = 0.5f;

    [FoldoutGroup("Platform"), HideInInspector] public float RightEdgeOfPlatform, LeftEdgeOfPlatform;
    public float FeedbackLimitDuration = 0.05f;

    public bool IsSuccess { get; set; }
    public const float COLLECTABLE_DIST_TO_GROUND = 0.35f;
    public const int NUM_OF_STACKABLE_TYPE = 3;

    private GameState _currentGameState;
    private MovementState _currentMovementState;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Initializations

    private void InitializeEdges()
    {
        Bounds platformBounds = _platformMesh.bounds;
        RightEdgeOfPlatform = platformBounds.max.x - _safeEdgeDistance;
        LeftEdgeOfPlatform = platformBounds.min.x + _safeEdgeDistance;
    }

    #endregion // Initializations

    #region StateControls

    public void SetMovStateTo(MovementState newState) => _currentMovementState = newState;

    public bool IsMovStateEqualsTo(MovementState thisState)
    {
        return thisState == _currentMovementState;
    }

    public bool IsMovStateEqualsTo(ComparerType comparer, params MovementState[] thisState)
    {
        bool isMovementStateEqualsOneOfThem = false;

        foreach (MovementState movState in thisState)
        {
            switch (comparer)
            {
                case ComparerType.Or:
                    isMovementStateEqualsOneOfThem |= movState == _currentMovementState;
                    break;

                case ComparerType.And:
                    isMovementStateEqualsOneOfThem &= movState == _currentMovementState;
                    break;
            }
        }

        return isMovementStateEqualsOneOfThem;
    }

    public MovementState GetMovState()
    {
        return _currentMovementState;
    }

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
        EventManager.Instance.StateLevelEnd += () => SetGameStateTo(GameState.LevelEnd);

        EventManager.Instance.FeverModeOn += () => SetMovStateTo(MovementState.Fever);
        EventManager.Instance.FeverModeOff += () => SetMovStateTo(MovementState.FreeToMove);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay -= () => SetGameStateTo(GameState.TapToPlay);
        EventManager.Instance.StateInGame -= () => SetGameStateTo(GameState.InGame);
        EventManager.Instance.StateEndingSequence -= () => SetGameStateTo(GameState.EndingSequence);
        EventManager.Instance.StateLevelSuccess -= () => SetGameStateTo(GameState.LevelSuccess);
        EventManager.Instance.StateLevelEnd -= () => SetGameStateTo(GameState.LevelEnd);

        EventManager.Instance.FeverModeOn -= () => SetMovStateTo(MovementState.Fever);
        EventManager.Instance.FeverModeOff -= () => SetMovStateTo(MovementState.FreeToMove);
    }

    #endregion // Events
}
