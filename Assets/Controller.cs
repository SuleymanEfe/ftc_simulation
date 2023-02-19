using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Controller : MonoBehaviour
{

    public Transform robot;

    public Camera cam1;
    public Camera cam2;
    public Camera robotCam;

    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float minDistance = 30f;
    [SerializeField] private float scrollSpeed = 4f;

    [SerializeField] private float distanceToTarget = 30;

    private Vector3 previousPosition;

    void Start()
    {
        disableAllCameras();

        robotCam.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            disableAllCameras();
            robotCam.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            disableAllCameras();
            cam1.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            disableAllCameras();
            cam2.enabled = true;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        distanceToTarget -= scroll * scrollSpeed;
        distanceToTarget = Mathf.Clamp(distanceToTarget, minDistance, maxDistance);

        robotCam.transform.position = robot.position;
        robotCam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = robotCam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = robotCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            robotCam.transform.position = robot.position;

            robotCam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            robotCam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

            robotCam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
    }

    void disableAllCameras()
    {
        cam1.enabled = false;
        cam2.enabled = false;
        robotCam.enabled = false;
    }

    // Toogle the state of being paused of simulation
    public void tooglePause()
    {
        Debug.Log(Time.timeScale);
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

}
