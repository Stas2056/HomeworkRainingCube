using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Exploder _exploder;
    [SerializeField] private RaycastTrigger _raycastTrigger;
    [SerializeField] private float _scaleDecrease = 2;
    [SerializeField] private int _spawnChanceDecrease = 2;
    [SerializeField] private int _spawnAmountMin = 2;
    [SerializeField] private int _spawnAmountMax = 6;

    private void OnEnable()
    {
        _raycastTrigger.CubeClicked += TrySpawn;
    }

    private void OnDisable()
    {
        _raycastTrigger.CubeClicked -= TrySpawn;
    }

    private void TrySpawn(Cube cube)
    {
        int minChance = 0;
        int maxChance = 100;
        int spawnRoll = Random.Range(minChance, maxChance);
        int spawnChance = cube.SpawnChance;

        if (spawnChance >= spawnRoll)
        {
            int newCubeSpawnChance = spawnChance / _spawnChanceDecrease;
            int spawnAmount = Random.Range(_spawnAmountMin, _spawnAmountMax);

            List<Rigidbody> rigidbodies = _spawner.Spawn(cube, spawnAmount, newCubeSpawnChance, _scaleDecrease);

            if (rigidbodies != null)
                _exploder.ExplodeChildrens(cube, rigidbodies);
        }
        else
        {
            _exploder.Explode(cube);
        }
    }
}