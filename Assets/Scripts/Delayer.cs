using System;
using System.Collections;
using UnityEngine;

public class Delayer : MonoBehaviour
{
    [SerializeField] private int _minTime = 2;
    [SerializeField] private int _maxTime = 5;


    public event Action<Cube> AfterDelay;

    private Coroutine _coroutine;

    public void ToDelay(Cube cube)
    {
        int delay = UnityEngine.Random.Range(_minTime, _maxTime + 1);

        _coroutine = StartCoroutine(Delay(delay, cube));
    }

    private IEnumerator Delay(int delay, Cube cube)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        AfterDelay?.Invoke(cube);
        StopCoroutine(_coroutine);
    }
}