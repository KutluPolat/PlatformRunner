using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;

[System.Serializable]
public class Stackable
{
    [SerializeField] private ParticleSystem _popParticles;
    public GameObject Model;
    public int Value;
    public StackableType StackableType;

    public void PlayParticles()
    {
        if(_popParticles != null)
        {
            _popParticles.transform.parent = null;
            _popParticles.Play();
        }
    }
}
