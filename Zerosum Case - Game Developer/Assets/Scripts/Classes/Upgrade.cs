using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Upgrade
{
    private const float PRICE_INCREMENT_RATE = 0.25f;
    private const int DEFAULT_PRICE = 25;

    public int CurrentPrice = DEFAULT_PRICE;
    public int Level = 1;

    protected virtual bool TryUpgrade(float price)
    {
        if (SaveSystem.Instance.TotalGold >= price)
        {
            SaveSystem.Instance.AddToTotalGold(-price);
            Level++;
            CurrentPrice = Mathf.FloorToInt(CurrentPrice * (1 + PRICE_INCREMENT_RATE));

            return true;
        }
        else
        {
            return false;
        }
    }
}
