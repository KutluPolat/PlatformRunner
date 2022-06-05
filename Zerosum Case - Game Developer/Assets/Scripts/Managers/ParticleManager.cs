using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ParticleManager : MonoBehaviour, IEvents
{
    #region Singleton

    public static ParticleManager Instance { get; private set; }

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

    [BoxGroup("Prefabs"), SerializeField]
    private GameObject _particleHolder;

    [BoxGroup("Particle System Pool"), SerializeField]
    private List<ParticleSystem> _particleList, _particleList2;

    [BoxGroup("Particle Attributes"), SerializeField]
    private float _minRadiusOfCircularParticles = 1.5f, _maxRadiusOfCircularParticles = 3f;

    public enum ParticleType
    {
        ParticleEnum,
        ParticleEnum2
    }

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Methods

    public void TriggerParticle(ParticleType particleType, Vector3 targetPosition, GameObject targetObject = null)
    {
        List<ParticleSystem> relatedParticleSystemList = GetRelatedListAccordingToParticleType(particleType);

        if (IterateParticleListAndFindInactiveParticle(relatedParticleSystemList, targetPosition, targetObject))
        {
            // Do Nothing
        }
        else
        {
            GameObject newParticle = Instantiate(relatedParticleSystemList[0].gameObject, Vector3.zero, relatedParticleSystemList[0].gameObject.transform.rotation, _particleHolder.transform);

            relatedParticleSystemList.Add(newParticle.GetComponent<ParticleSystem>());
            newParticle.SetActive(false);

            TriggerParticle(particleType, targetPosition);
        }
    }

    public void TriggerCircularParticle(ParticleType particleType, SphereCollider targetCollider, GameObject targetObject)
    {
        List<ParticleSystem> relatedParticleSystemList = GetRelatedListAccordingToParticleType(particleType);

        if (IterateParticleListAndFindInactiveParticle(relatedParticleSystemList, targetCollider, targetObject))
        {
            // Do Nothing
        }
        else
        {
            GameObject newParticle = Instantiate(relatedParticleSystemList[0].gameObject, Vector3.zero, relatedParticleSystemList[0].gameObject.transform.rotation, _particleHolder.transform);

            relatedParticleSystemList.Add(newParticle.GetComponent<ParticleSystem>());
            newParticle.SetActive(false);

            TriggerCircularParticle(particleType, targetCollider, targetObject);
        }
    }

    #endregion // Methods

    #region Sub-Methods

    private bool IterateParticleListAndFindInactiveParticle(List<ParticleSystem> particleSystems, SphereCollider sphereCollider, GameObject targetObject)
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            if (particle.gameObject.activeInHierarchy == false)
            {
                StartCoroutine(PlayCircularParticle(particle, sphereCollider, targetObject));
                return true;
            }
        }

        return false;
    }

    private bool IterateParticleListAndFindInactiveParticle(List<ParticleSystem> particleSystems, Vector3 targetPosition, GameObject targetObject)
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            if (particle.gameObject.activeInHierarchy == false)
            {
                StartCoroutine(PlayParticle(particle, targetPosition, targetObject));
                return true;
            }
        }

        return false;
    }

    private List<ParticleSystem> GetRelatedListAccordingToParticleType(ParticleType particleType)
    {
        switch (particleType)
        {
            case ParticleType.ParticleEnum:
                return _particleList;

            case ParticleType.ParticleEnum2:
                return _particleList2;
            default:
                Debug.LogWarning("Implement new type of particle.");
                return null;
        }
    }

    private IEnumerator PlayParticle(ParticleSystem particle, Vector3 targetPosition, GameObject targetObject)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = targetPosition;

        if(targetObject != null)
        {
            particle.gameObject.transform.rotation = targetObject.transform.rotation;
        }

        particle.Play();

        yield return new WaitForSeconds(2f);

        particle.gameObject.SetActive(false);
    }

    private IEnumerator PlayCircularParticle(ParticleSystem particle, SphereCollider allyCollider, GameObject targetObject)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = targetObject.transform.position;

        particle.gameObject.transform.localScale = Mathf.Clamp(allyCollider.radius, _minRadiusOfCircularParticles, _maxRadiusOfCircularParticles) * 2 * Vector3.one;

        if (targetObject != null)
        {
            particle.gameObject.transform.rotation = targetObject.transform.rotation;
        }

        particle.Play();

        yield return new WaitForSeconds(2f);

        particle.gameObject.SetActive(false);
    }

    #endregion Sub Methods

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
    }

    public void UnsubscribeEvents()
    {
    }

    #endregion // Events
}
