using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAroundAxis : MonoBehaviour
{
    public GameObject Fan1;

    public bool smallFan = false;

    void Update()
    {
        if (!smallFan)
        {
            transform.RotateAround(Fan1.transform.position, Vector3.up, 360 * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(Fan1.transform.position, Vector3.forward, 360 * Time.deltaTime);
        }

    }
}
