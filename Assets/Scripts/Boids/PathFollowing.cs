using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float speed;
    private int currentWaypointIndex = 0;
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
            currentWaypointIndex %= waypoints.Count;
        }
        transform.position += speed * Time.deltaTime *
            (waypoints[currentWaypointIndex].position - transform.position).normalized;
    }
}
