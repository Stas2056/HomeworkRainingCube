using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action Triggered;

    private bool _isTriggered;

    private void OnEnable()
    {
        _isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered == false)
        {
            Triggered?.Invoke();
            _isTriggered = true;
        }
    }
}
