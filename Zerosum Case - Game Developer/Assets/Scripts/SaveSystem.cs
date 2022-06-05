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

    public int TotalGold
    {
        get { return SaveSystemBinary<int>.Load("TotalGold", 0); }
        set { SaveSystemBinary<int>.Save("TotalGold", value); }
    }

    #endregion // Variables
}
