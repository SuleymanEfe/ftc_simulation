using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private bool isPaused = true;

    [SerializeField] private RobotController robotController;
    [SerializeField] private UIController uiController;

    private DataParser dataParser;
    private CameraController cameraController;

    [SerializeField] private GameObject mainMenuUI;

    public Transform robot;

    void Start()
    {
        dataParser = this.GetComponent<DataParser>();
        cameraController = this.GetComponent<CameraController>();

        cameraController.init(robot);

        //pauseSimulation();
    }

    private void Update()
    {
        uiController.progressBarCurrent = robotController.getCurrentIndex();

        if (Input.GetKeyDown("space")) tooglePaused();

        if (Input.GetKey(KeyCode.J)) robotController.moveOneFrameBackward();
        else if (Input.GetKey(KeyCode.K)) robotController.moveOneFrameForward();

        if (Input.GetKeyDown(KeyCode.Comma)) robotController.moveOneFrameBackward();
        else if (Input.GetKeyDown(KeyCode.Period)) robotController.moveOneFrameForward();
    }

    public void startSimulation()
    {
        isPaused = false;

        mainMenuUI.SetActive(false);

        List<Vector3> positions;
        List<long> timestamps;

        (positions, timestamps) = dataParser.parse(uiController.getFilePath());

        uiController.progressBarMax = dataParser.getDataSize();

        robotController.setPositionsData(positions);
        robotController.setTimestampsData(timestamps);

        robotController.startRobotMovement();
    }

    public void tooglePaused()
    {
        if (isPaused) continueSimulation();
        else pauseSimulation();
    }

    public void pauseSimulation()
    {
        isPaused = true;

        Time.timeScale = 0;
        robotController.stopRobotMovement();
    }

    public void continueSimulation()
    {
        isPaused = false;
        Time.timeScale = 1;
        robotController.startRobotMovement();
    }
}