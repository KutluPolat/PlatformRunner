using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField, SceneObjectsOnly] private Animator _rotAnim;

    private void LateUpdate()
    {
        _rotAnim.SetFloat("AvgInput", _movement.GetAverageOfLatestInputs());
    }
}
