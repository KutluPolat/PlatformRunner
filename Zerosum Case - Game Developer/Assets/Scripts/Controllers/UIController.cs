using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zerosum.PlatformRunner.Enums;
using TMPro;
using Sirenix.OdinInspector;

public class UIController : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField, FoldoutGroup("Variables")] private float _panelDelay = 2f;
    [SerializeField, FoldoutGroup("Panels")] private GameObject _tapToPlayPanel, _successPanel, _inGamePanel, _failPanel, _endingPanel;
    [SerializeField, FoldoutGroup("Backgrounds")] Image _failBackground, _successBackground;
    [SerializeField, FoldoutGroup("Texts")] private TextMeshProUGUI _levelText, _goldText;
    [SerializeField, FoldoutGroup("Buttons")] private GameObject _maxStackUpgrade, _startingStackUpgrade, _incomeUpgrade;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
        OpenTapToPlayPanel();
        UpdateGoldText();
    }

    #endregion // Start

    #region Button Controls

    public void OnPressedNextLevel() => EventManager.Instance.OnPressedNextLevel();
    public void OnPressedRestart() => EventManager.Instance.OnPressedRestart();

    private void OnPressedUpgradeButton(ButtonType pressedButton)
    {
        GameObject button = null;

        switch (pressedButton)
        {
            case ButtonType.StartingStack:
                button = _startingStackUpgrade;
                break;

            case ButtonType.MaxStack:
                button = _maxStackUpgrade;
                break;

            case ButtonType.Income:
                button = _incomeUpgrade;
                break;
        }

        DotweenExtensions.PunchScale(button.transform, DG.Tweening.Ease.OutBack, 1.3f, 1f, 0.25f);
    }

    #endregion // Button Controls

    #region Texts

    private void UpdateLevelText()
    {
        _levelText.text = "Level " + SaveSystem.Instance.CurrentLevel;
    }

    private void UpdateGoldText()
    {
        _goldText.text = $"{Mathf.FloorToInt(SaveSystem.Instance.TotalGold)}$";
    }

    #endregion // Texts

    #region Panel Controls

    private void OpenTapToPlayPanel()
    {
        CloseAllPanels();

        _tapToPlayPanel.SetActive(true);
    }

    private void OpenInGamePanel()
    {
        CloseAllPanels();

        _inGamePanel.SetActive(true);
        UpdateLevelText();
    }

    private void OpenFailPanel()
    {
        CloseAllPanels();

        _endingPanel.SetActive(true);
        _failPanel.SetActive(true);
        _failBackground.gameObject.SetActive(true);

        EventManager.Instance.OnStateLevelEnd();
    }

    private void OpenSuccessPanel()
    {
        CloseAllPanels();

        _endingPanel.SetActive(true);
        _successPanel.SetActive(true);
        _successBackground.gameObject.SetActive(true);

        EventManager.Instance.OnStateLevelEnd();
    }

    private void CloseAllPanels()
    {
        _tapToPlayPanel.SetActive(false);
        _successPanel.SetActive(false);
        _inGamePanel.SetActive(false);
        _failPanel.SetActive(false);
        _endingPanel.SetActive(false);
    }

    #endregion // Panel Control

    #region Panel Delays

    private IEnumerator DelayOpenLevelSuccess()
    {
        yield return new WaitForSeconds(_panelDelay);
        OpenSuccessPanel();
    }

    private IEnumerator DelayOpenLevelFail()
    {
        yield return new WaitForSeconds(_panelDelay);
        OpenFailPanel();
    }

    #endregion // Panel Delays

    #region Events

    public void SubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay += OpenTapToPlayPanel;
        EventManager.Instance.StateInGame += OpenInGamePanel;
        EventManager.Instance.StateLevelSuccess += () => { StartCoroutine(DelayOpenLevelSuccess()); };
        EventManager.Instance.StateLevelFailed += () => { StartCoroutine(DelayOpenLevelFail()); };

        EventManager.Instance.GoldUpdated += UpdateGoldText;
        EventManager.Instance.PressedUpgradeButton += OnPressedUpgradeButton;
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StateTapToPlay -= OpenTapToPlayPanel;
        EventManager.Instance.StateInGame -= OpenInGamePanel;
        EventManager.Instance.StateLevelSuccess -= () => { StartCoroutine(DelayOpenLevelSuccess()); };
        EventManager.Instance.StateLevelFailed -= () => { StartCoroutine(DelayOpenLevelFail()); };

        EventManager.Instance.GoldUpdated -= UpdateGoldText;
        EventManager.Instance.PressedUpgradeButton -= OnPressedUpgradeButton;
    }

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    #endregion // Events
}
