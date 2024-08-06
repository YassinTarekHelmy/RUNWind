using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Check : MonoBehaviour
{
    [SerializeField] protected LayerMask _checkedLayer;
    protected bool _validateCheck = false;
    protected bool _isEnabled = true;

    public bool ValidateCheck => _validateCheck;

    

    private void Update()
    {
        if (!_isEnabled) return;

        CheckIf();
    }

    public abstract void CheckIf();

    public void DisableCheck() {
        _isEnabled = false;
    }

    public void EnableCheck() {
        _isEnabled = true;
    }

    public void DisableCheckForTime(float time) {
        DisableCheck();
        _validateCheck = false;
        StartCoroutine(EnableCheckAfterTime(time));
    }

    private IEnumerator EnableCheckAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        EnableCheck();
    }
}
