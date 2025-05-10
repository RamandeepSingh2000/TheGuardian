using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boid : MonoBehaviour
{
    List<Boid> boids;
    private float localBoidDistance;
    private Transform flockTransform;
    private Vector3 moveDirection;
    private float speed;
    private float moveTowardsAverageStrength;
    private float moveToAvoidCollisionStrength;
    private float moveInAlignmentStrength;
    private float moveRandomStrength;
    private float pillarAvoidanceStrength;
    private List<Vector3> pillarLocations;
    private float pillarRadius;
    private float pillarAvoidanceRange;
    private bool initialised;
    public Vector3 MoveDirection => moveDirection;
    public void InjectDependencies(List<Boid> boids, float localBoidDistance, Transform flockTransform, float speed,
        float moveTowardsAverageStrength, float moveToAvoidCollisionStrength, float moveInAlignmentStrength, 
        float moveRandomStrength, float pillarAvoidanceStrength, List<Vector3> pillarLocations, float pillarRadius,
        float pillarAvoidanceRange)
    {
        this.boids = boids;
        this.localBoidDistance = localBoidDistance;
        this.flockTransform = flockTransform;
        this.speed = speed;
        this.moveTowardsAverageStrength = moveTowardsAverageStrength;
        this.moveToAvoidCollisionStrength = moveToAvoidCollisionStrength;
        this.moveInAlignmentStrength = moveInAlignmentStrength;
        this.moveRandomStrength = moveRandomStrength;
        this.pillarAvoidanceStrength = pillarAvoidanceStrength;
        this.pillarLocations = pillarLocations;
        this.pillarRadius = pillarRadius;
        this.pillarAvoidanceRange = pillarAvoidanceRange;
        initialised = true;
    }
    public void ChangeParameters(float localBoidDistance, float speed,
        float moveTowardsAverageStrength, float moveToAvoidCollisionStrength, float moveInAlignmentStrength,
        float moveRandomStrength, float pillarAvoidanceStrength)
    {
        this.localBoidDistance = localBoidDistance;
        this.speed = speed;
        this.moveTowardsAverageStrength = moveTowardsAverageStrength;
        this.moveToAvoidCollisionStrength = moveToAvoidCollisionStrength;
        this.moveInAlignmentStrength = moveInAlignmentStrength;
        this.moveRandomStrength = moveRandomStrength;
        this.pillarAvoidanceStrength = pillarAvoidanceStrength;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised)
        {
            return;
        }
        moveDirection = Vector3.zero;
        AddMovementTowardsAverageCenter();
        AddMovementToAvoidCollision();
        AddMovementForAlignment();
        AddRandomMovement();
        moveDirection.Normalize();
        transform.position += speed * Time.deltaTime * moveDirection;
        AvoidPillars();
    }
    private void AddMovementTowardsAverageCenter()
    {
        var positionSum = transform.position + flockTransform.position;
        int count = 2;
        foreach (Boid boid in boids)
        {
            if(Vector3.Distance(boid.transform.position, transform.position) < localBoidDistance)
            {
                positionSum += boid.transform.position;
                count++;
            }
        }
        var averagePosition = positionSum / count;
        var direction = (averagePosition - transform.position).normalized;
        moveDirection += direction * moveTowardsAverageStrength;
    }
    private void AddMovementToAvoidCollision()
    {
        Vector3 moveAway = Vector3.zero;
        foreach (Boid boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, transform.position) < localBoidDistance)
            {
                moveAway += transform.position - boid.transform.position;
            }
        }
        var moveAwayDirection = moveAway.normalized;
        moveDirection += moveAwayDirection * moveToAvoidCollisionStrength;
    }
    private void AddMovementForAlignment()
    {
        var moveDirectionSum = moveDirection;
        int count = 1;
        foreach (Boid boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, transform.position) < localBoidDistance)
            {
                moveDirectionSum += boid.MoveDirection;
                count++;
            }
        }
        var averageDirection = (moveDirectionSum / count).normalized;
        moveDirection += averageDirection * moveInAlignmentStrength;
    }
    private void AddRandomMovement()
    {
        var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        moveDirection += randomDirection.normalized * moveRandomStrength;     
    }
    private void AvoidPillars()
    {
        var pillarAvoidanceVector = Vector3.zero;
        foreach (var pillarLocation in pillarLocations)
        {
            var boidLocation2D = transform.position;
            boidLocation2D.y = 0f;
            var pillarLocation2D = pillarLocation;
            pillarLocation2D.y = 0f;
            if (Vector3.Distance(boidLocation2D, pillarLocation2D) > pillarAvoidanceRange)
            {
                continue;
            }

            var collisionPoint = pillarLocation + pillarRadius * (boidLocation2D - pillarLocation2D).normalized;
            pillarAvoidanceVector += boidLocation2D - collisionPoint;
        }
        transform.position += pillarAvoidanceStrength * speed * Time.deltaTime * pillarAvoidanceVector.normalized;
    }
    public void RemoveBoid()
    {
        boids.Remove(this);
    }
}
