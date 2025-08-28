using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GameObject : MonoBehaviour
{
    [SerializeField] private int _spawnChance;

    public event Action<GameObject> Clicked;

    public Rigidbody Rigidbody { get; private set; }
    public int SpawnChance => _spawnChance;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(int spawnChance, float scaleDecrease)
    {
        _spawnChance = spawnChance;
        transform.localScale /= scaleDecrease;
    }

    public void InvokeDestroy()
    {
        Clicked?.Invoke(this);
    }
}