using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptHandler : MonoBehaviour
{
    public void UnblockMovement()
    {
        EventManager.Instance.OnMovementUnblocked();
    }
}
