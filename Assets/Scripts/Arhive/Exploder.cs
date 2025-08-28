using System;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private int _baseExplosionForce = 2500;
    [SerializeField] private int _ExplosionForceModificator = 3;
    [SerializeField] private int _baseExplosionRadius = 50;

    public void ExplodeChildrens(GameObject cube, List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody spawnedCube in rigidbodies)
        {
            spawnedCube.AddExplosionForce(_baseExplosionForce, cube.transform.position,
                                          _baseExplosionRadius);
        }
    }

    private List<Rigidbody> GetRigidbodiesInExplosionRadius(float explosionRadius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.localPosition, explosionRadius);

        List<Rigidbody> rigidbodiesInExplosionRadius = new List<Rigidbody>();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                rigidbodiesInExplosionRadius.Add(hit.attachedRigidbody);
            }
        }

        return rigidbodiesInExplosionRadius;
    }

    public void Explode(GameObject cube)
    {
        float cubeScaleModificator = cube.transform.localScale.x;
        float explosionRadius = _baseExplosionRadius / cubeScaleModificator;
        float explosionForce = (_baseExplosionForce / cubeScaleModificator * _ExplosionForceModificator);
        float distance = 0;

        foreach (Rigidbody rigidbody in GetRigidbodiesInExplosionRadius(explosionRadius))
        {
            distance = Vector3.Distance(cube.transform.position, rigidbody.position);

            if (distance == 0)
                distance = 1;

            rigidbody.AddExplosionForce(explosionForce / distance, cube.transform.position, explosionRadius);
        }
    }
}