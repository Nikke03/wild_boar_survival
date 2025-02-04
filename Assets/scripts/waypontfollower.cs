using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypontfollower : MonoBehaviour
{

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointsIndex = 0;

    [SerializeField] private float speed = 2f;


    private void Update()
    {
        if(Vector2.Distance(waypoints[currentWaypointsIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointsIndex++;
            if(currentWaypointsIndex >= waypoints.Length)
            {
                currentWaypointsIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointsIndex].transform.position, Time.deltaTime * speed);

    }
}
