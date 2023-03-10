using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataParser : MonoBehaviour
{
    public List<Vector3> positions;
    public List<long> timestamps;

    public (List<Vector3>, List<long>) parse(string filePath)
    {
        string fileAsText = File.ReadAllText(filePath);
        RobotData JSONdata = JsonUtility.FromJson<RobotData>(fileAsText);

        positions = new List<Vector3>();
        timestamps = new List<long>();

        foreach (Position pos in JSONdata.pos)
        {
            float x = pos.x + RobotController.initialPoint.x;
            float z = pos.z + RobotController.initialPoint.z;

            positions.Add(new Vector3(x, 6.5f, z));
            timestamps.Add(pos.timestamp);
        }

        return (positions, timestamps);
    }

    public int getDataSize()
    {
        return positions.Count;
    }

    [System.Serializable]
    public class RobotData
    {
        public Position[] pos;
    }

    [System.Serializable]
    public class Position
    {
        public float x;
        public float z;
        public float heading;
        public long timestamp;
    }
}