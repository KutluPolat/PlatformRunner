using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;
using Sirenix.OdinInspector;
using DG.Tweening;

public class StackableController : MonoBehaviour
{
    [SerializeField, BoxGroup("Stackables")] private Stackable _money, _gold, _gem;
    [SerializeField, Space] private Collider _interactionCollider;
    [SerializeField] private float _colliderBlockingDuration = 0.25f, _destroyDuration = 2f;
    [SerializeField] private StackableType _currentStackableType;
    public bool IsCollected { get; set; }

    private void Awake()
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

        transform.position = new Vector3(transform.position.x, GameManager.COLLECTABLE_DIST_TO_GROUND, transform.position.z);
    }

    public Stackable GetCurrentStackable()
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
        if((int)_currentStackableType < GameManager.NUM_OF_STACKABLE_TYPE)
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
                EventManager.Instance.OnStackableExchanged(this);
                DelayedDestroy();
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

    public void DelayedDestroy()
    {
        StartCoroutine(DelayedDestroyCoroutine());
    }

    private IEnumerator DelayedDestroyCoroutine()
    {
        IsCollected = false;
        StartCoroutine(CloseColliderFor(_destroyDuration));
        transform.DOScale(0, _destroyDuration / 2f).OnComplete(() => { GetCurrentStackable().PlayParticles(); });
        
        yield return new WaitForSeconds(_destroyDuration);

        Destroy(gameObject);
    }
}
