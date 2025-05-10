using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] Boid boidPrefab;
    [SerializeField] Vector3 spawnAreaSize = new Vector3(10, 10, 10);
    [SerializeField] int boidCount = 20;

    [Header("Common Parameters")]
    [SerializeField] float localBoidDistance = 5f;
    [SerializeField] float speed = 5f;

    [Header("Strengths")]
    [SerializeField] [Range(0, 1f)] float moveTowardsAverageStrength = 1.0f;
    [SerializeField] [Range(0, 1f)] float moveToAvoidCollisionStrength = 1.0f;
    [SerializeField] [Range(0, 1f)] float moveInAlignmentStrength = 1.0f;
    [SerializeField] [Range(0, 1f)] float moveRandomStrength = 0.1f;
    [SerializeField] [Range(0, 1f)] float pillarAvoidanceStrength = 1f;

    [Header("Pillar Avoidance")]
    [SerializeField] List<Vector3> pillarLocations;
    [SerializeField] float pillarRadius;
    [SerializeField] float pillarAvoidanceRange = 5f;
    List<Boid> boids = new();

    [Header("Flock Center")]
    [SerializeField] Transform flockCentre;
    public void InjectPillarLocations(List<Vector3> pillarLocations)
    {
        this.pillarLocations = pillarLocations;
    }
    public void SpawnBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            var position = new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));

            var boid = Instantiate(boidPrefab, transform.position + position, Quaternion.identity, transform);
            boid.InjectDependencies(boids, localBoidDistance, flockCentre, speed, moveTowardsAverageStrength,
                moveToAvoidCollisionStrength, moveInAlignmentStrength, moveRandomStrength, pillarAvoidanceStrength,
                pillarLocations, pillarRadius, pillarAvoidanceRange);
            boids.Add(boid);
        }
    }
    private void OnValidate()
    {
        foreach(var boid in boids)
        {
            boid.ChangeParameters(localBoidDistance, speed, moveTowardsAverageStrength, moveToAvoidCollisionStrength,
                moveInAlignmentStrength, moveRandomStrength, pillarAvoidanceStrength);
        }
    }
}
