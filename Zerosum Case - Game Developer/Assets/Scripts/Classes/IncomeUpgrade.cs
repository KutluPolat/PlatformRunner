using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IncomeUpgrade : Upgrade
{
    public float IncomeMultiplier = 1;
    private const float INCOME_INCREASE_RATE = 0.2f;

    public bool UpgradeIncome()
    {
        if (base.TryUpgrade(CurrentPrice))
        {
            IncomeMultiplier += INCOME_INCREASE_RATE;
            return true;
        }
        else
        {
            return false;
        }
    }
}
