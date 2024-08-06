using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : Check
{
    [SerializeField] protected float _checkRadius = 0.4f;
    public override void CheckIf()
    {
        _validateCheck = Physics.CheckSphere(transform.position, _checkRadius, _checkedLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
