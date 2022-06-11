using Zerosum.PlatformRunner.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion // Singleton

    #region Variables

    [SerializeField] private AudioSource _oneShotAudioSource, _continiousAudioSource;
    [SerializeField] private AudioClip _audioOne, _audioTwo, _upgradeSucessfull, _upgradeFailed;

    private float _timer;

    #endregion // Variables

    #region Update

    private void Update()
    {
        if(_timer >= 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    #endregion // Update

    #region Methods

    #region Limited

    public void PlayOneShotLimitedAudio(AudioNames targetAudio)
    {
        if (_timer <= 0)
        {
            _timer = GameManager.Instance.FeedbackLimitDuration;
            PlayOneShotAudio(targetAudio);
        }
    }

    #endregion // Limited

    #region Instant

    public void PlayOneShotAudio(AudioNames targetAudio)
    {
        switch (targetAudio)
        {
            case AudioNames.AudioOne:
                Play(_audioOne);
                break;

            case AudioNames.AudioTwo:
                Play(_audioTwo);
                break;

            case AudioNames.UpgradeSucessfull:
                Play(_upgradeSucessfull);
                break;

            case AudioNames.UpgradeFailed:
                Play(_upgradeFailed);
                break;
        }
    }

    private void Play(AudioClip clip, float volumeScale = 1f)
    {
        if(clip != null && _oneShotAudioSource)
        {
            _oneShotAudioSource.PlayOneShot(clip, volumeScale);
        }
    }

    #endregion // Instant

    #region Instant (Pitch Tweaked)

    #endregion // Instant (Pitch Tweaked)

    #endregion // Methods
}
