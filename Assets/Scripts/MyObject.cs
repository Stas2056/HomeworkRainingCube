using UnityEngine;

public class MyObject : MonoBehaviour
{
    [SerializeField] public bool IsSubscribedToTrigger;
    [SerializeField] public bool IsSendToCoroutine;

    private void OnEnable()
    {
        IsSendToCoroutine = false;
        IsSubscribedToTrigger = false;
    }
}
