using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static MoveRobot;

public class RobotController : MonoBehaviour
{
    private List<Vector3> positions;
    private List<float> timestamps;
    private int currentIndex = 0;

    public void setPositionsData(List<Vector3> data)
    {
        positions = data;
    }

    public void setTimestampsData(List<float> data)
    {
        timestamps = data;
    }

    public void startRobotMovement()
    {
        StartCoroutine(MoveToNextPosition());
    }

    public void stopRobotMovement()
    {
        StopAllCoroutines();
    }

    public IEnumerator MoveToNextPosition()
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
}
