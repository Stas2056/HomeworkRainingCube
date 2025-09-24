using System;
using System.Collections;
using UnityEngine;

public class Delayer : MonoBehaviour
{
    [SerializeField] private int _minTime = 2;
    [SerializeField] private int _maxTime = 5;


    public event Action<UnityEngine.GameObject> AfterDelay;

    private Coroutine _coroutine;

    public void ToDelay(UnityEngine.GameObject cube)
    {
        int delay = UnityEngine.Random.Range(_minTime, _maxTime + 1);

        _coroutine = StartCoroutine(Delay(delay, cube));
    }

    private IEnumerator Delay(int delay, UnityEngine.GameObject cube)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        AfterDelay?.Invoke(cube);
        StopCoroutine(_coroutine);
    }
}