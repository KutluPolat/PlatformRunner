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
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion // Singleton

    #region Variables

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

    #endregion // Variables

    #region Methods

    public void AddGold(float value)
    {
        TotalGold += value;

        if (value > 0)
            EventManager.Instance.OnGoldCollected();
        else
            EventManager.Instance.OnGoldLost();
    }

    #endregion // Methods
}
