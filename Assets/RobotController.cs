using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static MoveRobot;

public class RobotController : MonoBehaviour
{
    public TextAsset jsonFile;
    private List<Vector3> positions;
    private List<float> timestamps;
    private int currentIndex = 0;

    void Start()
    {
    }

    void OnEnable()
    {
        ParseJsonFile();
        StartCoroutine(MoveToNextPosition());
    }

    void ParseJsonFile()
    {
        // Parse the JSON string and create a list of positions
        PositionData positionData = JsonUtility.FromJson<PositionData>(jsonFile.text);

        positions = new List<Vector3>();
        timestamps = new List<float>();

        foreach (Position pos in positionData.pos)
        {
            float x = pos.x;
            float z = pos.z;
            positions.Add(new Vector3(x, 6.5f, z));
            timestamps.Add(pos.timestamp);
        }
    }

    IEnumerator MoveToNextPosition()
    {
        while (true)
        {
            Vector3 nextPosition = positions[currentIndex];

            // Vector3 currentPosition = transform.position;
            // Vector3 direction = (nextPosition - currentPosition).normalized;
            // transform.position = currentPosition + direction * speed * Time.deltaTime;

            transform.position = nextPosition;

            // Check if the robot has reached the next position
            if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
            {
                currentIndex = (currentIndex + 1) % positions.Count;
            }

            if (currentIndex == positions.Count - 1) currentIndex = 0;

            yield return new WaitForSeconds(timestamps[currentIndex + 1] - timestamps[currentIndex]);
        }
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
