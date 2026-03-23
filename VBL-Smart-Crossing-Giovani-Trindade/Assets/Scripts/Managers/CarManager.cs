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

    void InitializeCars()
    {
        for (int i = 0; i < carPoolSize; i++)
        {
            GameObject instantiatedCar = Instantiate(carPrefab, poolTransform.position, Quaternion.identity);
            carPool.Add(instantiatedCar);
        }
    }

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

    void RemoveCar(object data = null)
    {
        Car carToBeRemoved = (Car)data;
        carToBeRemoved.Deactivate();
        carToBeRemoved.transform.position = poolTransform.position;

        carPool.Add(carToBeRemoved.gameObject);
    }
}
