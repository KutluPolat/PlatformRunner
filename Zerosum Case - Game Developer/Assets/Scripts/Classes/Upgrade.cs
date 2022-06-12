using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Upgrade
{
    // Reuseable code

    private const float DEFAULT_PRICE = 25, PRICE_INCREMENT_RATE = 0.25f;
    public float CurrentPrice;
    public int Level;

    public Upgrade()
    {
        CurrentPrice = CalculatePrice(Level);
    }

    protected virtual bool TryUpgrade(float price)
    {
        if(SaveSystem.Instance.TotalGold >= price)
        {
            SaveSystem.Instance.AddToTotalGold(-price);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected float CalculatePrice(int skillLevel)
    {
        return DEFAULT_PRICE + DEFAULT_PRICE * (Mathf.Clamp(skillLevel - 1, 0, Mathf.Infinity) * (1 + PRICE_INCREMENT_RATE));
    }

    public void Save(string saveName, Upgrade savedClass)
    {
        SaveSystemBinary<Upgrade>.Save(saveName, savedClass);
    }
}
