using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform robot;

    public Camera topViewCam;
    public Camera robotViewCam;

    private float maxDistance = 50f;
    private float minDistance = 30f;
    private float scrollSpeed = 5f;
    private float distanceToTarget = 50;

    private Vector3 previousPosition;

    public void init(Transform robot)
    {
        this.robot = robot;

        disableAllCameras();
        robotViewCam.enabled = true;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            disableAllCameras();
            robotViewCam.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            disableAllCameras();
            topViewCam.enabled = true;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        distanceToTarget -= scroll * scrollSpeed;
        distanceToTarget = Mathf.Clamp(distanceToTarget, minDistance, maxDistance);

        robotViewCam.transform.position = robot.position;
        robotViewCam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = robotViewCam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = robotViewCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            robotViewCam.transform.position = robot.position;

            robotViewCam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            robotViewCam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

            robotViewCam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
    }


    void disableAllCameras()
    {
        topViewCam.enabled = false;
        robotViewCam.enabled = false;
    }


}
