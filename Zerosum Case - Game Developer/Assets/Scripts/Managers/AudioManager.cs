using Coati.GameName.Enums;
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
    [SerializeField] private AudioClip _audioOne, _audioTwo;

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

    public void PlayOneShotLimitedAudio(AudioNames targetAudio)
    {
        if(_timer <= 0)
        {
            _timer = GameManager.Instance.FeedbackLimitDuration;
            PlayOneShotAudio(targetAudio);
        }
    }

    #region Limited

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

    #endregion // Methods
}
