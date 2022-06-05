using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;
using Sirenix.OdinInspector;

public class StackableController : MonoBehaviour
{
    [SerializeField, BoxGroup("Stackables")] private Stackable _money, _gold, _gem;
    [SerializeField, Space] private Collider _interactionCollider;
    [SerializeField] private float _colliderBlockingDuration = 0.25f;
    [SerializeField] private StackableType _currentStackableType;
    [HideInInspector] public bool IsCollected;

    private void Start()
    {
        InitializeStackable();
    }

    private void InitializeStackable()
    {
        _money.Model.SetActive(false);
        _gold.Model.SetActive(false);
        _gem.Model.SetActive(false);

        switch (_currentStackableType)
        {
            case StackableType.Money:
                _money.Model.SetActive(true);
                break;

            case StackableType.Gold:
                _gold.Model.SetActive(true);
                break;

            case StackableType.Gem:
                _gem.Model.SetActive(true);
                break;
        }
    }


    private Stackable GetCurrentStackable()
    {
        switch (_currentStackableType)
        {
            case StackableType.Money:
                return _money;

            case StackableType.Gold:
                return _gold;

            case StackableType.Gem:
                return _gem;

            default:
                Debug.LogError("Don't forget to implement new type!!");
                Debug.Break();
                return null;
        }
    }

    private void UpgradeStackable()
    {
        if((int)_currentStackableType < GameManager.NumOfStackableType)
        {
            switch (_currentStackableType)
            {
                case StackableType.Money:

                    _currentStackableType = StackableType.Gold;

                    _money.Model.SetActive(false);
                    _gold.Model.SetActive(true);

                    break;

                case StackableType.Gold:

                    _currentStackableType = StackableType.Gem;

                    _gold.Model.SetActive(false);
                    _gem.Model.SetActive(true);

                    break;

                default:
                    Debug.LogError("Don't forget to implement new type!");
                    Debug.Break();
                    break;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsCollected)
        {
            if (other.CompareTag("Upgrader"))
            {
                StartCoroutine(CloseColliderFor(_colliderBlockingDuration));
                UpgradeStackable();
            }
            else if (other.CompareTag("Obstacle"))
            {
                EventManager.Instance.OnStackTouchedToTheObstacle(this);
            }
            else if (other.CompareTag("Stackable"))
            {
                StackableController otherStackable = other.GetComponent<StackableController>();

                if(otherStackable.IsCollected == false)
                {
                    EventManager.Instance.OnStackCollected(otherStackable);
                }
            }
            else if (other.CompareTag("Exchanger")) 
            {
                Stackable currentStackable = GetCurrentStackable();

                currentStackable.PlayParticles();
                EventManager.Instance.OnStackableExchanged(currentStackable);
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                EventManager.Instance.OnStackCollected(this);
            }
        }
    }

    private IEnumerator CloseColliderFor(float seconds)
    {
        _interactionCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        _interactionCollider.enabled = true;
    }
}
