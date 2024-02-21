using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaypointScript : MonoBehaviour
{
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float moveSpeed = 0.008f;
    private float originalMoveSpeed;
    [SerializeField] [Range(0, 1f)] private float rotationSpeed = 0.22f;
    private Vector3 Destination;
    private bool reachedDestination;


    private void Start()
    {
       // transform.position = GameManagerPvp.instance.PlayerVcam.transform.position;
        Destination = currentWaypoint.GetPosition();
        transform.position = Destination;

    }

    void Update()
    {
       

        if (currentWaypoint != null)
        {
         
                Vector3 destinationDirection = Destination - transform.position;

                //destinationDirection.y = 0;

                  float destinationDistance = destinationDirection.magnitude;

               // float destinationDistance = Vector3.Distance(transform.position, Destination);
               
                if (destinationDistance >= stopDistance)
                {

                    reachedDestination = false;

                  //  Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);

                   // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

                    transform.position = Vector3.MoveTowards(transform.position, Destination, moveSpeed);

                }
                else
                {
                
                
                    reachedDestination = true;

                    currentWaypoint = currentWaypoint.nextWaypoint;
                    Destination = currentWaypoint.GetPosition();
                   // moveSpeed = originalMoveSpeed;

                }

            


        }
    }

}
