using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CollectableHandler : MonoBehaviour
{
    [SerializeField] private Transform _modelParent;
    [SerializeField] private Collectable _collectable;

    [SerializeField, MinMaxSlider(0, 5f), Tooltip("Defines min and max values for specified randomizations.")] 
    private Vector2 _yDist = new Vector2(2f, 3f), _duration = new Vector2(0.5f, 1f);

    private void OnCollected()
    {
        float randDuration = Random.Range(_duration.x, _duration.y);
        float randYDist = Random.Range(_yDist.x, _yDist.y);

        _modelParent.DOScale(0, randDuration).SetEase(Ease.OutBack);
        _modelParent.DOLocalMoveY(randYDist, randDuration).OnComplete(() => 
        {
            if(_collectable.CollectParticles != null)
            {
                _collectable.CollectParticles.transform.parent = null;
                _collectable.CollectParticles.Play();
            }

            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Stackable"))
        {
            GetComponent<Collider>().enabled = false;
            OnCollected();
            SaveSystem.Instance.AddGold(_collectable.Value);
        }
    }
}
