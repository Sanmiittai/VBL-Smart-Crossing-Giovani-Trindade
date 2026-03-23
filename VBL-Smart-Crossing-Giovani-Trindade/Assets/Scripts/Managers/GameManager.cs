using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] FloatVariable playerSpeedMultiplier;
    [SerializeField] FloatVariable averageSpeed;
    [SerializeField] FloatVariable nextStatusTime;
    [SerializeField] FloatVariable levelTime;

    [SerializeField] IntVariable currentLevel;

    float spawnInterval = 1;
    float spawnTimer;
    float crossingTimer;
    float nextStatusTimer;

    bool gameStarted;

    Status currentStatus;

    TrafficResponse currentTraffic;
    TrafficResponse nextTraffic;

    List<PredictedStatusItem> nextStatuses = new List<PredictedStatusItem>();

    void OnEnable()
    {
        EventManager.AddListener(EventType.StatusChanged, NewStatusReceived);
        EventManager.AddListener(EventType.LevelAdvance, AdvanceLevel);
        EventManager.AddListener(EventType.GameOver, OnGameOver);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.StatusChanged, NewStatusReceived);
        EventManager.RemoveListener(EventType.LevelAdvance, AdvanceLevel);
        EventManager.RemoveListener(EventType.GameOver, OnGameOver);
    }


    void Start()
    {
        currentLevel.value = 1;
    }

    void AdvanceLevel(object data = null)
    {
        currentLevel.value++;
        if (currentLevel.value > 5)
            GameWin();
        else
            ChangeCurrentTraffic(nextTraffic);
    }

    void NewStatusReceived(object data = null)
    {
        if (currentStatus == null)
        {
            ChangeCurrentTraffic((TrafficResponse)data);
        }
        else
        {
            nextTraffic = (TrafficResponse)data;
        }
    }

    void ChangeCurrentTraffic(TrafficResponse trafficToChange)
    {
        currentTraffic = trafficToChange;

        if (currentTraffic.predicted_status.Count() > 0)
        {
            PredictedStatusItem[] predictedStatuses = currentTraffic.predicted_status;
            nextStatuses.Clear();
            nextStatuses = predictedStatuses.ToList();

            levelTime.value = nextStatuses[^1].estimated_time / 1000;

            crossingTimer = 0;
        }

        UpdateStatus(currentTraffic.current_status, true);
    }

    void UpdateStatus(Status nextStatus, bool isTrafficChange)
    {
        currentStatus = nextStatus;
        averageSpeed.value = currentStatus.averageSpeed;
        spawnInterval = 0.5f / (currentStatus.vehicleDensity * 2);

        switch (currentStatus.weather)
        {
            case "sunny":
                playerSpeedMultiplier.value = 1f;
                break;
            case "clouded":
                playerSpeedMultiplier.value = 0.8f;
                break;
            case "foggy":
                playerSpeedMultiplier.value = 0.8f;
                break;
            case "light rain":
                playerSpeedMultiplier.value = 0.6f;
                break;
            case "heavy rain":
                playerSpeedMultiplier.value = 0.4f;
                break;
        }

        if (!isTrafficChange)
            nextStatuses.RemoveAt(0);

        nextStatusTime.value = nextStatuses[0].estimated_time / 1000;
    }

    void Update()
    {
        CheckSpawnCar();

        if (!gameStarted) return;

        CheckGameOverTime();

        CheckNextStatusTime();
    }

    void CheckNextStatusTime()
    {
        nextStatusTimer += Time.deltaTime;

        if (nextStatusTimer >= nextStatusTime.value)
        {
            nextStatusTimer = 0;
            UpdateStatus(nextStatuses[0].prediction, false);
        }
    }

    void CheckGameOverTime()
    {
        crossingTimer += Time.deltaTime;

        if (crossingTimer >= levelTime.value)
        {
            EventManager.InvokeEvent(EventType.GameOver);
        }
    }

    void CheckSpawnCar()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            EventManager.InvokeEvent(EventType.SpawnCar);
        }
    }

    void OnGameOver(object data = null)
    {
        gameStarted = false;
    }

    void GameWin()
    {

    }
}
