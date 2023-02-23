using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static DataParser;

public class RobotController : MonoBehaviour
{
    private List<Vector3> positions;
    private List<long> timestamps;
    private int currentIndex = 0;

    [SerializeField] public static Vector3 initialPoint = new Vector3(0, 6.5f, -300);


    public void setPositionsData(List<Vector3> data)
    {
        positions = data;
    }

    public void setTimestampsData(List<long> data)
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

            transform.position = nextPosition;

            currentIndex = (currentIndex + 1) % positions.Count;
            int nextIndex = (currentIndex + 1) % positions.Count;

            DateTimeOffset dateTime1 = DateTimeOffset.FromUnixTimeMilliseconds(timestamps[nextIndex]);
            DateTimeOffset dateTime2 = DateTimeOffset.FromUnixTimeMilliseconds(timestamps[currentIndex]);

            long secondsBetween = (long)(dateTime1 - dateTime2).TotalMilliseconds;

            yield return new WaitForSeconds((float)(secondsBetween / 1000.0));
        }
    }

    public void moveOneFrameForward()
    {
        if (currentIndex == positions.Count - 1) return;

        currentIndex++;
        transform.position = positions[currentIndex];
    }

    public void moveOneFrameBackward()
    {
        if (currentIndex == 0) return;
        currentIndex--;
        transform.position = positions[currentIndex];
    }

    public int getCurrentIndex()
    {
        return currentIndex;
    }
}
