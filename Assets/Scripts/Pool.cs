using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    //создать пулл +
    //изменение цвета +-
    //таймер самоуничтожения
    //рандомизация спауна
    //создать платформы

    [SerializeField] private Delayer _delayer;
    [SerializeField] private UnityEngine.GameObject _prefab;
    [SerializeField] private UnityEngine.GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

   public event Action<UnityEngine.GameObject> ToDelay;

    private ObjectPool<UnityEngine.GameObject> _pool;
  //  private Dictionary<UnityEngine.GameObject, bool> _isSend = new Dictionary<UnityEngine.GameObject, bool>();


    private void Awake()
    {
        _pool = new ObjectPool<UnityEngine.GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        _delayer.AfterDelay += ReleaseCube;
    }

    private void OnDisable()
    {
        _delayer.AfterDelay -= ReleaseCube;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void ActionOnGet(UnityEngine.GameObject obj)
    {
        obj.transform.position = _startPoint.transform.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
                
       //     _isSend.Add(obj, false);
        
        Trigger trigger = obj.GetComponent<Trigger>();
        // Debug.Log("subscribe");
        trigger.Triggered += () => TimeDelayedRelease(obj);
    }

    private void GetCube()
    {
        //  Debug.Log("active " + _pool.CountActive + " inactive " + _pool.CountInactive + " all " + _pool.CountAll);

        if (_pool.CountActive < _poolMaxSize)
        {
            _pool.Get();
        }
    }

    private void TimeDelayedRelease(UnityEngine.GameObject cube)
    {
       // Debug.Log("TRY send to cor from pool " + _isSend[cube]);

       // if (_isSend[cube] == false)
       // {
            Debug.Log("send to cor from pool");
           ToDelay?.Invoke(cube);
       //     _isSend[cube]=true;
      //  }
    }

    private void ReleaseCube(UnityEngine.GameObject cube)
    {
        if (cube.activeInHierarchy)
        {
            //  Debug.Log("release cube");
            _pool.Release(cube);
         //   _isSend.Remove(cube);
            Trigger trigger = cube.GetComponent<Trigger>();
            // Debug.Log("UnSubscribe");
            trigger.Triggered -= () => ReleaseCube(cube);
        }
    }
}