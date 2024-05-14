using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints to follow
    public float moveSpeed = .5f; // Speed of movement
    private int currentWaypointIndex = 0; // Index of the current waypoint

    private void Update()
    {
        transform.Rotate(new Vector3(-4, -4, -8) * Time.deltaTime);
        // Check if there are waypoints defined
        if (waypoints.Length > 0)
        {
            // Get the current waypoint
            Transform currentWaypoint = waypoints[currentWaypointIndex];

            // Move towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

            // Check if the character has reached the current waypoint
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }
}

