using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCamera;
    public float yRotationOffset = 0f;

    private void Update()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("No target camera assigned to FaceCamera script.");
            return;
        }

        // Get the direction from the object to the camera
        Vector3 direction = targetCamera.transform.position - transform.position;

        // Calculate the rotation needed to face the camera
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Adjust the rotation to only rotate around the Y-axis
        //rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);

        // Adjust the rotation to add the offset to the Y-axis
        rotation *= Quaternion.Euler(0f, yRotationOffset, 0f);

        // Apply the rotation to the object
        transform.rotation = rotation;
    }
}
