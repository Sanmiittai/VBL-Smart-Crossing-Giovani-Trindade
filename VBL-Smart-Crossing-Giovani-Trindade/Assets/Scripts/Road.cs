using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public Crossing[] crossings;

    void Awake()
    {
        foreach(var crossing in crossings)
        {
            crossing.InitializeSpawnPoints();
        }
    }
}

[System.Serializable]
public class Crossing
{
    public Transform spawnPointParent;
    public List<Transform> spawnPoints;
    public Vector3 crossingDirection;

    public void InitializeSpawnPoints()
    {
        foreach (Transform spawnPosition in spawnPointParent)
        {
            spawnPoints.Add(spawnPosition);
        }
    }
}
