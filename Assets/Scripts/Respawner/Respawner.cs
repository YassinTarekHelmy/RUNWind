using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.PostProcessing;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> _respawnableObjects;

    [Header("Rewind Settings")]
    [SerializeField] private float _rewindSpeed = 10f;
    [SerializeField] private float _stopThreshold = 0.1f;
    
    [Header("References")]
    [SerializeField] private GameObject _rewindUI;

    private bool _isRespawning = false;

    public Dictionary<GameObject, Vector3> RespawnableObjects { get => _respawnableObjects; set => _respawnableObjects = value; }
    private void Start() {
        _respawnableObjects = FindObjectsOfType<Rigidbody>().Select(rb => rb.gameObject).ToDictionary(rb => rb, rb => rb.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isRespawning = true;

        }
    }

    private void Update() {
        if (_isRespawning)
        {
            foreach (var obj in _respawnableObjects)
            {
                StartCoroutine(Rewind(obj.Key.transform, obj.Value));
            }

            _isRespawning = false;
        }
           
    }

    private IEnumerator Rewind(Transform currentObject, Vector3 initialPosition)
    {
        if (currentObject.CompareTag("Player"))
        {
            _rewindUI.SetActive(true);
            PostProcessor.Instance.SetVHSWeight(1);
        }
        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        while (Vector3.Distance(currentObject.position, initialPosition) > _stopThreshold)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            currentObject.SetPositionAndRotation(Vector3.Lerp(currentObject.position, initialPosition, _rewindSpeed * Time.deltaTime),
                                                Quaternion.Lerp(currentObject.rotation, Quaternion.identity, _rewindSpeed * Time.deltaTime));
            yield return null;
        }

        currentObject.position = initialPosition;

        if (!currentObject.TryGetComponent(out FallingPlatform  fallingPlatform))
        {
            rb.isKinematic = false;
        }

        if (currentObject.CompareTag("Player"))
        {
            _rewindUI.SetActive(false);
            PostProcessor.Instance.SetVHSWeight(0);
        }
    }

}
