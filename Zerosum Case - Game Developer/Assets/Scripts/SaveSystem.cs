using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    #region Singleton

    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StackedGold = 0;
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    #endregion // Singleton

    #region Variables

    public const int DEFAULT_MAX_NUM_OF_STACKABLES = 10;

    public int CurrentLevel
    {
        get { return SaveSystemBinary<int>.Load("Level", 1); }
        set { SaveSystemBinary<int>.Save("Level", value); }
    }

    public float TotalGold
    {
        get { return SaveSystemBinary<float>.Load("TotalGold", 0); }
        private set { SaveSystemBinary<float>.Save("TotalGold", value); }
    }

    public float StackedGold
    {
        get { return SaveSystemBinary<float>.Load("StackedGoldInThisLevel", 0); }
        private set { SaveSystemBinary<float>.Save("StackedGoldInThisLevel", value); }
    }

    public int CurrentNumOfStack
    {
        get { return SaveSystemBinary<int>.Load("CurrentNumOfStack", 0); }
        set { SaveSystemBinary<int>.Save("CurrentNumOfStack", value); }
    }

    public int MaxNumOfStack
    {
        get { return MaxStackUpgrades.MaxNumOfStack; }
    }

    public int StartingNumOfStack
    {
        get { return StartingStackUpgrades.StartingStack; }
    }

    public IncomeUpgrade IncomeUpgrades
    {
        get { return SaveSystemBinary<IncomeUpgrade>.Load("IncomeUpgrades", new IncomeUpgrade()); }
        set { SaveSystemBinary<IncomeUpgrade>.Save("IncomeUpgrades", value); }
    }

    public StartingStackUpgrade StartingStackUpgrades
    {
        get { return SaveSystemBinary<StartingStackUpgrade>.Load("StartingStackUpgrades", new StartingStackUpgrade()); }
        set { SaveSystemBinary<StartingStackUpgrade>.Save("StartingStackUpgrades", value); }
    }

    public MaxStackUpgrade MaxStackUpgrades
    {
        get { return SaveSystemBinary<MaxStackUpgrade>.Load("MaxStackUpgrades", new MaxStackUpgrade()); }
        set { SaveSystemBinary<MaxStackUpgrade>.Save("MaxStackUpgrades", value); }
    }

    #endregion // Variables

    #region Methods

    public void AddToTotalGold(float value)
    {
        TotalGold += value;

        if (value > 0)
        {
            EventManager.Instance.OnGoldCollected();
        }
        else
        {
            EventManager.Instance.OnGoldLost();
        }
    }

    public void AddToStackedGold(float value)
    {
        StackedGold += value;

        if (value > 0)
            EventManager.Instance.OnGoldCollected();
        else
            EventManager.Instance.OnGoldLost();
    }

    #endregion // Methods
}
