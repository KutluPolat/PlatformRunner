using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingStackUpgrade : Upgrade
{
    public int StartingStack = 0;

    public bool UpgradeStartingStack()
    {
        if (base.TryUpgrade(CurrentPrice))
        {
            StartingStack++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
