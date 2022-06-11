using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.OnMovementBlocked();
            EventManager.Instance.OnPlayerTrapped(null);
        }
        else if (other.CompareTag("Stackable"))
        {
            EventManager.Instance.OnStackableTrapped(other.GetComponent<StackableController>());
        }
    }
}
