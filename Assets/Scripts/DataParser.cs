using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataParser : MonoBehaviour
{
    public (List<Vector3>, List<float>) parse(string filePath)
    {
        string fileAsText = File.ReadAllText(filePath);
        RobotData JSONdata = JsonUtility.FromJson<RobotData>(fileAsText);

        List<Vector3> positions = new List<Vector3>();
        List<float> timestamps = new List<float>();

        foreach (Position pos in JSONdata.pos)
        {
            float x = pos.x;
            float z = pos.z;

            positions.Add(new Vector3(x, 6.5f, z));
            timestamps.Add(pos.timestamp);
        }

        return (positions, timestamps);
    }

    [System.Serializable]
    public class RobotData
    {
        public Position[] pos;
        public float timestamp;
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
