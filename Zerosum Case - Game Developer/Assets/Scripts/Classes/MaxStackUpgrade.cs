using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaxStackUpgrade : Upgrade
{
    public int MaxNumOfStack = SaveSystem.DEFAULT_MAX_NUM_OF_STACKABLES;

    public bool UpgradeMaxStack()
    {
        if (base.TryUpgrade(CurrentPrice))
        {
            MaxNumOfStack += 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}
