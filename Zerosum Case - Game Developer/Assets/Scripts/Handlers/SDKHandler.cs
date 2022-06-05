//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ElephantSDK;
//using UnityEngine.Analytics;

//public class SDKHandler : MonoBehaviour, IEvents
//{
//    #region Variables

//    [SerializeField] private bool _isSDKOpen;

//    #endregion // Variables

//    #region Start

//    void Start()
//    {
//        SubscribeEvents();

//        UpdateLevelLoadUnityAnalytics();
//        UpdateLevelStartedCounter();
//    }

//    #endregion // Start

//    #region Methods

//    private void UpdateLevelLoadUnityAnalytics()
//    {
//        if (_isSDKOpen)
//        {
//            AnalyticsResult analyticsResult = Analytics.CustomEvent("LevelLoad", new Dictionary<string, object> {
//            { "Level", SaveSystem.Instance.Level },
//            { (SaveSystem.Instance.Level).ToString() +" Level_Orj:", SaveSystem.Instance.IndexOfLevel }});
//        }
//        else
//        {
//            Debug.Log("Load UA");
//        }
//    }

//    private void UpdateLevelCompletedUnityAnalytics()
//    {
//        if (_isSDKOpen)
//        {
//            AnalyticsResult analyticsResult = Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object> {
//                { "Level", SaveSystem.Instance.Level },
//                { (SaveSystem.Instance.Level).ToString() +" Level_Orj:", SaveSystem.Instance.IndexOfLevel }});
//        }
//        else
//        {
//            Debug.Log("Success UA");
//        }
//    }

//    private void UpdateLevelFailedUnityAnalytics()
//    {
//        if (_isSDKOpen)
//        {
//            AnalyticsResult analyticsResult = Analytics.CustomEvent("LevelFailed", new Dictionary<string, object> {
//                { "Level", SaveSystem.Instance.Level },
//                { (SaveSystem.Instance.Level).ToString() +" Level_Orj:", SaveSystem.Instance.IndexOfLevel }});
//        }
//        else
//        {
//            Debug.Log("Failed UA");
//        }
//    }

//    private void UpdateLevelStartedCounter()
//    {
//        if (_isSDKOpen)
//        {
//            Elephant.LevelStarted(SaveSystem.Instance.Level);
//        }
//        else
//        {
//            Debug.Log("Loaded Elephant");
//        }
//    }

//    private void UpdateLevelCompletedCounter()
//    {
//        if (_isSDKOpen)
//        {
//            Elephant.LevelCompleted(SaveSystem.Instance.Level);
//        }
//        else
//        {
//            Debug.Log("Success Elephant");
//        }
//    }

//    private void UpdateLevelFailedCounter()
//    {
//        if (_isSDKOpen)
//        {
//            Elephant.LevelFailed(SaveSystem.Instance.Level);
//        }
//        else
//        {
//            Debug.Log("Failed Elephant");
//        }
//    }

//    #endregion // Methods

//    #region Events

//    public void SubscribeEvents()
//    {
//        EventManager.Instance.StateLevelSuccess += UpdateLevelCompletedUnityAnalytics;
//        EventManager.Instance.StateLevelSuccess += UpdateLevelCompletedCounter;

//        EventManager.Instance.StateLevelFailed += UpdateLevelFailedCounter;
//        EventManager.Instance.StateLevelFailed += UpdateLevelFailedUnityAnalytics;
//    }

//    public void UnsubscribeEvents()
//    {
//        EventManager.Instance.StateLevelSuccess -= UpdateLevelCompletedUnityAnalytics;
//        EventManager.Instance.StateLevelSuccess -= UpdateLevelCompletedCounter;

//        EventManager.Instance.StateLevelFailed -= UpdateLevelFailedCounter;
//        EventManager.Instance.StateLevelFailed -= UpdateLevelFailedUnityAnalytics;
//    }

//    public void OnDestroy()
//    {
//        UnsubscribeEvents();
//    }

//    #endregion // Events
//}