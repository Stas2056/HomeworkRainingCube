using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    //создать пулл
    //рандомизация спауна
    //таймер самоуничтожения
    //изменение цвета
    //создать платформы

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = _startPoint.transform.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);

        Trigger trigger = obj.GetComponent<Trigger>();
        Debug.Log("subscribe");
        trigger.Triggered += () => TimeDelayedRelease(obj);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        Debug.Log("active " + _pool.CountActive+ " inactive " + _pool.CountInactive+ " all " + _pool.CountAll);
       
        if (_pool.CountActive < _poolMaxSize)
        {
            _pool.Get();
        }
    }

    private void TimeDelayedRelease(GameObject cube)
    {
        Invoke("ReleaseCube", 2f);
    }

    private void ReleaseCube(GameObject cube)
    {
        Debug.Log("release cube");
        Trigger trigger = cube.GetComponent<Trigger>();
        Debug.Log("UnSubscribe");
        trigger.Triggered -= () => ReleaseCube(cube);

    }

}