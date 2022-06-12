using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;

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

        EventManager.Instance.OnPressedUpgradeButton(ButtonType.StartingStack);
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

        EventManager.Instance.OnPressedUpgradeButton(ButtonType.MaxStack);
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

        EventManager.Instance.OnPressedUpgradeButton(ButtonType.Income);
    }

    private void OnUpgradeSuccessfull()
    {
        UpdateSaves();
    }
    private void UpdateSaves()
    {
        AudioManager.Instance.PlayOneShotAudio(AudioNames.UpgradeSucessfull, false, false);
        SaveSystem.Instance.MaxStackUpgrades = _maxStackUpgrade;
        SaveSystem.Instance.StartingStackUpgrades = _startingStackUpgrade;
        SaveSystem.Instance.IncomeUpgrades = _incomeUpgrade;
    }

    private void OnUpgradeFailed()
    {
        AudioManager.Instance.PlayOneShotAudio(AudioNames.UpgradeFailed, false, false);
    }
}
