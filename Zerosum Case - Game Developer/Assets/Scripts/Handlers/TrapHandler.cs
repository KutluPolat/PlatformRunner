using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHandler : MonoBehaviour
{
    private StackableController _triggeredStackableControllerCache;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.OnMovementBlocked();
            EventManager.Instance.OnPlayerTrapped(null);
        }
        else if (other.CompareTag("Stackable"))
        {
            _triggeredStackableControllerCache = other.GetComponent<StackableController>();

            if (_triggeredStackableControllerCache.IsCollected)
                EventManager.Instance.OnStackableTrapped(_triggeredStackableControllerCache);
        }
    }
}
