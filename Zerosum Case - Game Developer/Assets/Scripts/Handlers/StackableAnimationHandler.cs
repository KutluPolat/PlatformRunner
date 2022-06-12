using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    public void OnCollected() => _anim.Play("Collected");
    public void OnIdle() => _anim.Play("Idle");
}
