using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private RobotController robotController;
    [SerializeField] private UIController uiController;

    private DataParser dataParser;

    [SerializeField] private GameObject mainMenuUI;

    public Transform robot;

    public Camera topViewCam;
    public Camera robotViewCam;

    private float maxDistance = 50f;
    private float minDistance = 30f;
    private float scrollSpeed = 4f;
    private float distanceToTarget = 30;

    private Vector3 previousPosition;

    void Start()
    {
        dataParser = this.AddComponent<DataParser>();

        pauseGame();

        disableAllCameras();
        robotViewCam.enabled = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            disableAllCameras();
            robotViewCam.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
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

    public void startGame()
    {
        mainMenuUI.SetActive(false);

        List<Vector3> positions;
        List<float> timestamps;

        (positions, timestamps) = dataParser.parse(uiController.getFilePath());

        robotController.setPositionsData(positions);
        robotController.setTimestampsData(timestamps);

        robotController.startRobotMovement();
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        robotController.stopRobotMovement();
    }

    public void continueGame()
    {
        Time.timeScale = 1;
        robotController.startRobotMovement();
    }
}