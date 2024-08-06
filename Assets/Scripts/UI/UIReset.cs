using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIReset : MonoBehaviour
{
    private Dictionary<GameObject,Vector3> _respawnableObjects;
    private void Start() {
        _respawnableObjects = FindObjectsOfType<Rigidbody>().Select(rb => rb.gameObject).ToDictionary(rb => rb, rb => rb.transform.position);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Respawnable")) {
            other.transform.position = _respawnableObjects[other.gameObject];
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
