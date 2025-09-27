using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool IsSubscribedToTrigger;
    public bool IsSendToCoroutine;

    private void OnEnable()
    {
        IsSendToCoroutine = false;
        IsSubscribedToTrigger = false;
    }
}
