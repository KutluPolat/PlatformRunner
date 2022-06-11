using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    private StartingStackUpgrade _startingStackUpgrade;
    private IncomeUpgrade _incomeUpgrade;
    private MaxStackUpgrade _maxStackUpgrade;

    private void Start()
    {
        _startingStackUpgrade = SaveSystem.Instance.StartingStackUpgrades;
        _incomeUpgrade = SaveSystem.Instance.IncomeUpgrades;
        _maxStackUpgrade = SaveSystem.Instance.MaxStackUpgrades;
    }

    public void OnPressedStartingStackUpgradeButton()
    {
        if (_startingStackUpgrade.UpgradeStartingStack())
        {
            OnUpgradeSuccessfull();
        }
        else
        {
            OnUpgradeFailed();
        }
    }

    public void OnPressedMaxStackUpgradeButton()
    {
        if (_maxStackUpgrade.UpgradeMaxStack())
        {
            OnUpgradeSuccessfull();
        }
        else
        {
            OnUpgradeFailed();
        }
    }

    public void OnPressedIncomeUpgradeButton()
    {
        if (_incomeUpgrade.UpgradeIncome())
        {
            OnUpgradeSuccessfull();
        }
        else
        {
            OnUpgradeFailed();
        }
    }

    private void OnUpgradeSuccessfull()
    {
        UpdateSaves();
        Debug.Log("Success");
    }
    private void UpdateSaves()
    {
        SaveSystem.Instance.MaxStackUpgrades = _maxStackUpgrade;
        SaveSystem.Instance.StartingStackUpgrades = _startingStackUpgrade;
        SaveSystem.Instance.IncomeUpgrades = _incomeUpgrade;
    }

    private void OnUpgradeFailed()
    {
        Debug.Log("Failed");
    }

    
}
