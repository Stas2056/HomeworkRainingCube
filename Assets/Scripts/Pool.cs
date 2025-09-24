using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _prefab;
    [SerializeField] private UnityEngine.GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<UnityEngine.GameObject> _pool;

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

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void ActionOnGet(UnityEngine.GameObject obj)
    {
        obj.transform.position = RandomisePosition(_startPoint.transform.position);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);

        Trigger trigger = obj.GetComponent<Trigger>();
        trigger.Triggered += () => TimeDelayedRelease(obj);
    }

    private Vector3 RandomisePosition(Vector3 position)
    {
        int rangeX = 5;
        int rangeZ = 5;
        int rangeY = 1;

        position.x += RandomRange(rangeX);
        position.z += RandomRange(rangeZ);
        position.y += RandomRange(rangeY);

        return position;
    }

    private int RandomRange(int range)
    {
        return UnityEngine.Random.Range(-range, +range + 1);
    }

    private void GetCube()
    {
        if (_pool.CountActive < _poolMaxSize)
        {
            _pool.Get();
        }
    }

    private void TimeDelayedRelease(UnityEngine.GameObject obj)
    {
        MyObject myObject = obj.GetComponent<MyObject>();
        Delayer delayer = obj.GetComponent<Delayer>();

        if (myObject.IsSendToCoroutine == false)
        {
            delayer.AfterDelay += ReleaseCube;
            delayer.ToDelay(obj);

            myObject.IsSendToCoroutine = true;
        }
    }

    private void ReleaseCube(UnityEngine.GameObject obj)
    {
        Delayer delayer = obj.GetComponent<Delayer>();

        if (obj.activeInHierarchy)
        {
            _pool.Release(obj);
            Trigger trigger = obj.GetComponent<Trigger>();
            trigger.Triggered -= () => TimeDelayedRelease(obj);
            delayer.AfterDelay -= ReleaseCube;
        }
    }
}