using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCube), 0.0f, _repeatRate);
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = RandomisePosition(_startPoint.transform.position);
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject cubeObject = cube.gameObject;
        cube.gameObject.SetActive(true);

        Trigger trigger = cube.GetComponent<Trigger>();
        trigger.Triggered += () => TimeDelayedRelease(cube);
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
        return Random.Range(-range, +range + 1);
    }

    private void SpawnCube()
    {
        if (_pool.CountActive < _poolMaxSize)
        {
            _pool.Get();
        }
    }

    private void TimeDelayedRelease(Cube cube)
    {
        Delayer delayer = cube.GetComponent<Delayer>();

        if (cube.IsSendToCoroutine == false)
        {
            delayer.AfterDelay += ReleaseCube;
            delayer.ToDelay(cube);

            cube.IsSendToCoroutine = true;
        }
    }

    private void ReleaseCube(Cube cube)
    {
        Delayer delayer = cube.GetComponent<Delayer>();

        if (cube.gameObject.activeInHierarchy)
        {
            _pool.Release(cube);
            Trigger trigger = cube.GetComponent<Trigger>();
            trigger.Triggered -= () => TimeDelayedRelease(cube);
            delayer.AfterDelay -= ReleaseCube;
        }
    }
}