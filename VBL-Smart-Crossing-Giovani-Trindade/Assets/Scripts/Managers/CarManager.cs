using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class CarManager : MonoBehaviour
{
    [SerializeField] IntVariable currentLevel;

    [SerializeField] Transform poolTransform;

    [SerializeField] GameObject carPrefab;

    [SerializeField] int carPoolSize;

    [SerializeField] Road[] roads;

    List<GameObject> carPool = new List<GameObject>();

    void OnEnable()
    {
        EventManager.AddListener(EventType.SpawnCar, SpawnCarOnLevel);
        EventManager.AddListener(EventType.RemoveCar, RemoveCar);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.SpawnCar, SpawnCarOnLevel);
        EventManager.RemoveListener(EventType.RemoveCar, RemoveCar);
    }

    void Start()
    {
        InitializeCars();
    }

    //Instantiates enough cars to fill the pool out of the game playable area
    void InitializeCars()
    {
        for (int i = 0; i < carPoolSize; i++)
        {
            GameObject instantiatedCar = Instantiate(carPrefab, poolTransform.position, Quaternion.identity);
            carPool.Add(instantiatedCar);
        }
    }

    //Spawns cars in the level in all current crossings and sets their direction
    //and position based on the crossing variables
    //Removes the car from the pool
    void SpawnCarOnLevel(object data = null)
    {
        foreach (var crossing in roads[currentLevel.value - 1].crossings)
        {
            Transform[] currentCrossingSpawnPoints = crossing.spawnPoints.ToArray();

            int randomSpawnPointIndex = Random.Range(0, currentCrossingSpawnPoints.Count());
            Transform randomSpawnPoint = currentCrossingSpawnPoints[randomSpawnPointIndex];

            Car carToBeAdded = carPool[0].GetComponent<Car>();

            carToBeAdded.SetDirection(crossing.crossingDirection);
            carToBeAdded.ChangeSpeed();
            carToBeAdded.transform.position = randomSpawnPoint.position;

            carPool.RemoveAt(0);
        }
    }

    //Removes the car from the game when reaching the car limiter
    //Deactivates the car and puts it back in the pool
    void RemoveCar(object data = null)
    {
        Car carToBeRemoved = (Car)data;
        carToBeRemoved.Deactivate();
        carToBeRemoved.transform.position = poolTransform.position;

        carPool.Add(carToBeRemoved.gameObject);
    }
}
