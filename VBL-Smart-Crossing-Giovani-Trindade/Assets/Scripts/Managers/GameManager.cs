using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] FloatVariable playerSpeedMultiplier;
    [SerializeField] FloatVariable averageSpeed;
    [SerializeField] FloatVariable nextStatusTime;

    [SerializeField] IntVariable currentLevel;

    float levelTime;
    float spawnInterval = 1;
    float spawnTimer;
    float crossingTimer;
    float nextStatusTimer;

    bool gameStarted;
    bool gameWon;

    Status currentStatus;

    TrafficResponse currentTraffic;
    TrafficResponse nextTraffic;

    List<PredictedStatusItem> nextStatuses = new List<PredictedStatusItem>();

    void OnEnable()
    {
        EventManager.AddListener(EventType.StatusChanged, NewStatusReceived);
        EventManager.AddListener(EventType.LevelAdvance, AdvanceLevel);
        EventManager.AddListener(EventType.GameStart, (object data) => gameStarted = true);
        EventManager.AddListener(EventType.GameOver, OnGameOver);
        EventManager.AddListener(EventType.GameWin, GameWin);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.StatusChanged, NewStatusReceived);
        EventManager.RemoveListener(EventType.LevelAdvance, AdvanceLevel);
        EventManager.RemoveListener(EventType.GameStart, (object data) => gameStarted = true);
        EventManager.RemoveListener(EventType.GameOver, OnGameOver);
        EventManager.RemoveListener(EventType.GameWin, GameWin);
    }


    void Start()
    {
        currentLevel.value = 1;
    }

    void AdvanceLevel(object data = null)
    {
        currentLevel.value++;
        if (currentLevel.value > 5)
            EventManager.InvokeEvent(EventType.GameWin);
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
        currentStatus = currentTraffic.current_status;

        if (currentTraffic.predicted_status.Count() > 0)
        {
            PredictedStatusItem[] predictedStatuses = currentTraffic.predicted_status;
            nextStatuses.Clear();
            nextStatuses = predictedStatuses.ToList();

            levelTime = nextStatuses[^1].estimated_time / 1000;

            EventManager.InvokeEvent(EventType.LevelHUDUpdate, levelTime);

            crossingTimer = 0;
        }

        UpdateStatus(currentStatus, true);
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
        {
            nextStatuses.RemoveAt(0);
        }

        nextStatusTime.value = nextStatuses[0].estimated_time / 1000;

        EventManager.InvokeEvent(EventType.HUDUpdate, currentStatus);
        EventManager.InvokeEvent(EventType.AverageSpeedChange);

        if (!gameStarted) EventManager.InvokeEvent(EventType.APILoaded);
    }

    void Update()
    {
        if (gameWon) return;

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
            UpdateStatus(nextStatuses[0].predictions, false);
        }
    }

    void CheckGameOverTime()
    {
        crossingTimer += Time.deltaTime;

        if (crossingTimer >= levelTime)
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

    void GameWin(object data = null)
    {
        gameWon = true;
    }
}
