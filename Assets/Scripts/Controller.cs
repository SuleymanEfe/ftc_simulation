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
    private CameraController cameraController;

    [SerializeField] private GameObject mainMenuUI;

    public Transform robot;

    void Start()
    {
        dataParser = this.AddComponent<DataParser>();
        cameraController = this.GetComponent<CameraController>();

        cameraController.init(robot);

        pauseSimulation();
    }

    public void startSimulation()
    {
        mainMenuUI.SetActive(false);

        List<Vector3> positions;
        List<float> timestamps;

        (positions, timestamps) = dataParser.parse(uiController.getFilePath());

        robotController.setPositionsData(positions);
        robotController.setTimestampsData(timestamps);

        robotController.startRobotMovement();
    }

    public void pauseSimulation()
    {
        Time.timeScale = 0;
        robotController.stopRobotMovement();
    }

    public void continueSimulation()
    {
        Time.timeScale = 1;
        robotController.startRobotMovement();
    }
}