using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class Delayer : MonoBehaviour
{
    [SerializeField] private Pool _pool;
    [SerializeField] private int _minTime = 2;
    [SerializeField] private int _maxTime = 5;


    public event Action<UnityEngine.GameObject> AfterDelay;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _pool.ToDelay += ToDelay;
    }

    private void OnDisable()
    {
        _pool.ToDelay -= ToDelay;
    }

    private void ToDelay(UnityEngine.GameObject cube)
    {

        // Debug.Log("cor accepted");
        int delay = UnityEngine.Random.Range(_minTime, _maxTime);

        _coroutine = StartCoroutine(Delay(delay, cube));
    }

    private IEnumerator Delay(int delay, UnityEngine.GameObject cube)
    {
        Debug.Log("cor start");
        var wait = new WaitForSeconds(delay);

        yield return wait;

        Debug.Log("cor send action");
        AfterDelay?.Invoke(cube);
        StopCoroutine(_coroutine);
        Debug.Log("cor end");
    }

    //private IEnumerator Delay(int delay, UnityEngine.GameObject cube)
    //{
    //    //Debug.Log("cor start");
    //    var wait = new WaitForSeconds(1f);

    //    for (int i = 0; i <= delay; i++)
    //    {
    //        // Debug.Log("cor counter " +i+ "delay "+delay);

    //        if (i >= delay)
    //        {
    //            Debug.Log("cor send action");
    //            AfterDelay?.Invoke(cube);
    //            StopCoroutine(_coroutine);
    //        }

    //        yield return wait;
    //    }

    //    Debug.Log("cor end");

    //}
}