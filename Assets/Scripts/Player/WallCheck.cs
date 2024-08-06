using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : Check
{
    [SerializeField] private Transform _orientation;

    [SerializeField] private float _maxCheckDistance = 1f;
    [SerializeField] private RaycastHit _wallData;

    public override void CheckIf()
    {
        _validateCheck = Physics.Raycast(transform.position, _orientation.right, out _wallData, _maxCheckDistance, _checkedLayer) ||
                         Physics.Raycast(transform.position, -_orientation.right, out _wallData, _maxCheckDistance, _checkedLayer);
    }
    
    public Vector3 GetNormal() {
        if (!_validateCheck) {
            return Vector3.zero;
        } else {
            return _wallData.normal;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(transform.position, transform.position - _orientation.right * _maxCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + _orientation.right * _maxCheckDistance);
    }
}
