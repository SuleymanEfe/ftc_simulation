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

    //public void update()
    //{
    //    float moveHorizontal = Input.GetAxis("Horizontal"); // Gets the horizontal input from the user's keyboard
    //    float moveVertical = Input.GetAxis("Vertical"); // Gets the vertical input from the user's keyboard

    //    Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical); // Creates a vector with the x and z components based on the user's input
    //    this.GetComponent<Rigidbody>().velocity = movement * 5f; // Applies the movement to the object's Rigidbody component
    //}

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
