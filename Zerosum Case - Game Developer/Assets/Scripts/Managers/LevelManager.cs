using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField] private int _targetLevel;
    [SerializeField] private LevelLoadingType _levelLoadingType = LevelLoadingType.DontLoad;

    public int TargetOriginalIndex
    {
        get
        {
            if (SaveSystem.Instance.CurrentLevel <= NumberOfLevels)
            {
                return SaveSystem.Instance.CurrentLevel - 1;
            }
            else
            {
                return GetRandomLevelIndex();
            }
        }
    }
    public int TargetOriginalLevel { get { return TargetOriginalIndex + 1; } }
    public static int SpecifiedTargetLevel, NumberOfLevels;

    private static Stack<int> _randomIndexes = new Stack<int>();
    private static bool _isApplicationActive;
    

    #endregion // Variables

    #region Start

    private void Start()
    {
        InitializeNumberOfLevels();
        SubscribeEvents();
        InitializeLevel();

        EventManager.Instance.OnStateTapToPlay();
    }

    #endregion // Start

    #region Initializations

    private void InitializeLevel()
    {
        switch (_levelLoadingType)
        {
            case LevelLoadingType.Normal:

                Instantiate(Resources.Load<GameObject>("Levels/Level " + TargetOriginalLevel));

                break;

            case LevelLoadingType.ContinueFromSpecifiedLevel:

                if (_targetLevel == 0)
                {
                    Debug.LogError($"Target Level should be in the following range [1, {NumberOfLevels}]");
                }
                else
                {
                    if (_isApplicationActive == false)
                    {
                        _isApplicationActive = true;
                        SpecifiedTargetLevel = _targetLevel;
                    }

                    Instantiate(Resources.Load<GameObject>("Levels/Level " + SpecifiedTargetLevel));
                }

                break;

            case LevelLoadingType.AlwaysLoadSpecifiedLevel:

                if (_targetLevel == 0)
                {
                    Debug.LogError($"Target Level should be in the following range [1, {NumberOfLevels}]");
                }
                else
                {
                    SpecifiedTargetLevel = _targetLevel;
                    Instantiate(Resources.Load<GameObject>("Levels/Level " + SpecifiedTargetLevel));
                }

                break;
        }
    }

    private void InitializeNumberOfLevels() => NumberOfLevels = Resources.LoadAll("Levels/").Length;

    #endregion // Initializations

    #region Methods

    public void NextLevel()
    {
        if (_randomIndexes.Count == 0)
            ProduceRandomIndexes();

        SaveSystem.Instance.CurrentLevel++;
        _randomIndexes.Pop();

        SpecifiedTargetLevel++;
        if (SpecifiedTargetLevel == NumberOfLevels + 1)
        {
            SpecifiedTargetLevel = 1;
        }

        Restart();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private int GetRandomLevelIndex()
    {
        if (_randomIndexes.Count == 0)
            ProduceRandomIndexes();

        return _randomIndexes.Peek();
    }

    private void ProduceRandomIndexes()
    {
        List<int> levelIndexes = new List<int>();

        for (int i = 0; i < NumberOfLevels; i++)
        {
            levelIndexes.Add(i);
        }

        for (int i = 0; i < NumberOfLevels; i++)
        {
            int random = Random.Range(0, levelIndexes.Count);

            _randomIndexes.Push(levelIndexes[random]);

            levelIndexes.Remove(random);
        }
    }

    #endregion // Methods

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.PressedNextLevel += NextLevel;
        EventManager.Instance.PressedRestart += Restart;
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.PressedNextLevel -= NextLevel;
        EventManager.Instance.PressedRestart -= Restart;
    }

    #endregion // Events
}
