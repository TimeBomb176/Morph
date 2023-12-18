using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform movingPlatformObject;

    [Tooltip("The amount of time the platform stays stationary before moving on to the next point in the array")]
    [SerializeField] private float platformWaitTime = 5f;

    [Tooltip("The distance the platform needs to be from the next point in the array before starting the Platform Wait Time")]
    [SerializeField] private float maxPlatformDistance = .01f;

    [Tooltip("The speed at which the platform will move from point to point in the Moving Platform Path Array")]
    [SerializeField] private float platformMoveSpeed = .02f;

    [Tooltip("The point in the array that the platform will start at")]
    [SerializeField] private int startingPointInPathArray = 0;

    [Tooltip("The path of point the platform will move to")]
    [SerializeField] private Transform[] movingPlatformPathArray;


    private float defaultPlatformWaitTime;



    private void Start()
    {
        defaultPlatformWaitTime = platformWaitTime;

        if (movingPlatformPathArray != null && movingPlatformPathArray.Length >= 1)
        {
            movingPlatformObject.position = movingPlatformPathArray[0].position;
        }
    }

    private void Update()
    {
        if (movingPlatformPathArray != null && movingPlatformPathArray.Length >= 1)
        {
            Vector3 currentPlatformPos = movingPlatformObject.position;
            Vector3 currentArrayPointPos = movingPlatformPathArray[startingPointInPathArray].position;

            if (Vector3.Distance(currentArrayPointPos, currentPlatformPos) < maxPlatformDistance)
            {
                platformWaitTime -= Time.deltaTime;

                if (platformWaitTime <= 0)
                {
                    platformWaitTime = defaultPlatformWaitTime;
                    startingPointInPathArray++;
                }

                if (startingPointInPathArray >= movingPlatformPathArray.Length)
                {
                   startingPointInPathArray = 0;
                }
            }


            Vector3 newPos = Vector3.MoveTowards(currentPlatformPos, currentArrayPointPos, platformMoveSpeed);
            movingPlatformObject.position = newPos;
        } else
        {
            Debug.LogWarning("Missing Moving Platform Objector Moving Platform Path Array is null");
        }

    }


}
