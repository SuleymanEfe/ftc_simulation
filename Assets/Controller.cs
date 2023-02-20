using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Controller : MonoBehaviour
{
    [SerializeField] private RobotController robotController;
    [SerializeField] private UIController uiController;

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

        parseJsonFile();

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

    public void parseJsonFile()
    {
        string fileAsText = File.ReadAllText(uiController.getFilePath());
        PositionData positionData = JsonUtility.FromJson<PositionData>(fileAsText);

        List<Vector3> positions = new List<Vector3>();
        List<float> timestamps = new List<float>();

        foreach (Position pos in positionData.pos)
        {
            float x = pos.x;
            float z = pos.z;
            positions.Add(new Vector3(x, 6.5f, z));
            timestamps.Add(pos.timestamp);
        }

        robotController.setPositionsData(positions);
        robotController.setTimestampsData(timestamps);
    }

    [System.Serializable]
    public class PositionData
    {
        public Position[] pos;
    }

    [System.Serializable]
    public class Position
    {
        public float x;
        public float z;
        public float heading;
        public float timestamp;
    }
}
