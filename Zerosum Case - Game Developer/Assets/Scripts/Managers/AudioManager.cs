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
            ResetPitch();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion // Singleton

    #region Variables

    [SerializeField] private AudioSource _oneShotAudioSource, _oneShotPitchTweakedAudioSource;
    [SerializeField] private AudioClip _collect, _upgradeSucessfull, _upgradeFailed;

    [SerializeField] private float _defaultPitch = 0.4f, _targetPitch = 1.25f, _pitchTweakSpeed = 0.05f, _pitchResetDuration = 1f;

    private float _timer, _pitchTimer, _currentPitch;


    #endregion // Variables

    #region Update

    private void Update()
    {
        if(_timer >= 0)
        {
            _timer -= Time.deltaTime;
        }

        if(_pitchTimer >= 0)
        {
            _pitchTimer -= Time.deltaTime;

            if(_pitchTimer < 0)
            {
                ResetPitch();
            }
        }
    }

    #endregion // Update

    #region Methods

    private void ResetPitch() => _currentPitch = _defaultPitch;

    #region Limited

    public void PlayOneShotAudio(AudioNames targetAudio, bool isLimited, bool isPitchTweaked)
    {
        if (isLimited)
        {
            if (_timer <= 0)
            {
                _timer = GameManager.Instance.FeedbackLimitDuration;
                PlayOneShotAudio(targetAudio, isPitchTweaked);
            }
        }
        else
        {
            PlayOneShotAudio(targetAudio, isPitchTweaked);
        }
    }

    #endregion // Limited

    #region Instant

    private void PlayOneShotAudio(AudioNames targetAudio, bool isPitchTweaked)
    {
        switch (targetAudio)
        {
            case AudioNames.Collect:
                Play(_collect, isPitchTweaked);
                break;

            case AudioNames.UpgradeSucessfull:
                Play(_upgradeSucessfull, isPitchTweaked);
                break;

            case AudioNames.UpgradeFailed:
                Play(_upgradeFailed, isPitchTweaked);
                break;
        }
    }

    private void Play(AudioClip clip, bool isPitchTweaked = false, float volumeScale = 1f)
    {
        if (isPitchTweaked)
        {
            if (clip != null && _oneShotPitchTweakedAudioSource)
            {
                _pitchTimer = _pitchResetDuration;

                _currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, _pitchTweakSpeed);
                _oneShotPitchTweakedAudioSource.pitch = _currentPitch;

                _oneShotPitchTweakedAudioSource.PlayOneShot(clip, volumeScale);
            }
        }
        else
        {
            if (clip != null && _oneShotAudioSource)
            {
                _oneShotAudioSource.PlayOneShot(clip, volumeScale);
            }
        }
    }

    #endregion // Instant

    #endregion // Methods
}
