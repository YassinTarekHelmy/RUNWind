using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public static CustomGravity Instance { get; private set; }

    [Header("Gravity Settings")]
    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private float _opposingForceMultiplier = 1.5f;
    [SerializeField] private float _timeScale = 1f;

    private float _opposingForce = 1f;
    private float _opposingTorque = 1f;

    public float GravityForce { get => _gravityForce; }
    public float TimeScale { get => _timeScale; set => _timeScale = value; }

    private Rigidbody[] _rigidbodies;

    private void Awake() {
        if (Instance != this && Instance != null) {
            Destroy(this);
        } else {
            Instance = this;
        }

        
    }

    private void Start()
    {
         _rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];

        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.useGravity = false;
            
            if (rb.gameObject.CompareTag("Player")) 
                continue;
                
            MonoBehaviour monoBehaviour = rb.GetComponent<MonoBehaviour>();
            
            monoBehaviour.StartCoroutine(CustomGravityRoutine(rb));
        
        }
    }

    private IEnumerator CustomGravityRoutine(Rigidbody rb)
    {
        while (true)
        {
            // Apply custom gravity
            rb.AddForce(_gravityForce * _timeScale * Vector3.down, ForceMode.Acceleration);

            if (Mathf.Abs(_timeScale - 1f) > 0.05f)
            {
                // Calculate the current velocity vector
                Vector3 currentVelocity = rb.velocity;

                // Calculate the target reduced velocity based on the time scale
                Vector3 targetVelocity = currentVelocity * _timeScale;
                
                if (rb.velocity.magnitude > targetVelocity.magnitude) {                    

                    // Calculate the opposing force vector to slow down the current velocity towards the target velocity
                    Vector3 opposingForce = (targetVelocity - currentVelocity) * _opposingForce;

                    // Apply the opposing force
                    rb.AddForce(opposingForce * _opposingForceMultiplier, ForceMode.Acceleration);

                    // Calculate the current angular velocity vector
                    Vector3 currentAngularVelocity = rb.angularVelocity;

                    // Calculate the target reduced angular velocity based on the time scale
                    Vector3 targetAngularVelocity = currentAngularVelocity * _timeScale;

                    if (rb.angularVelocity.magnitude > targetAngularVelocity.magnitude)
                    {
                        // Calculate the opposing torque vector to slow down the current angular velocity towards the target angular velocity
                        Vector3 opposingTorque = (targetAngularVelocity - currentAngularVelocity) * _opposingTorque;

                        // Apply the opposing torque
                        rb.AddTorque(opposingTorque * _opposingForceMultiplier, ForceMode.Acceleration);
                    }
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
