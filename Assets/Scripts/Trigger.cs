using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action Triggered;

    private void OnTriggerEnter(Collider other)
    {
        Triggered?.Invoke();
       // gameObject.SetActive(false);
    }
}
