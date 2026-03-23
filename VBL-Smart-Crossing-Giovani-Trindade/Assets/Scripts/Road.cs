using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public Crossing[] crossings;

    //Initialize all crossings spawn points
    void Awake()
    {
        foreach (var crossing in crossings)
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

    //Adds every spawn point inside the spawn point parent to the list
    public void InitializeSpawnPoints()
    {
        foreach (Transform spawnPosition in spawnPointParent)
        {
            spawnPoints.Add(spawnPosition);
        }
    }
}
