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

    //Resets the current level on scene start
    void Start()
    {
        currentLevel.value = 1;
    }

    //Advances to the next level, changing the current traffic to the pre-loaded next traffic
    //Or calling the GameWin event if the last level was cleared
    void AdvanceLevel(object data = null)
    {
        currentLevel.value++;
        if (currentLevel.value > 5)
            EventManager.InvokeEvent(EventType.GameWin);
        else
            ChangeCurrentTraffic(nextTraffic);
    }

    //Receives the data from API Manager with the Traffic Response
    //If it's the first call, calls ChangeCurrentTraffic to set current traffic to received data
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

    //Updates the current status and next statuses to the received data
    //Calls the LevelHUDUpdate event to change the level on the HUD
    //Calls UpdateStatus to update the game current variables to the new status variables
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

    //Updates the game current variables to the new status variables
    //Calls the HUDUpdate event o update the hud with the new variables
    //Calls the AverageSpeedChange to update the cars average speed
    //If it's the first time being called, calls the APILoaded event to allow the game to start
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

    //Checks if the time to change the status for the next status 
    //has been met, and if true calls UpdateStatus with the next status
    void CheckNextStatusTime()
    {
        nextStatusTimer += Time.deltaTime;

        if (nextStatusTimer >= nextStatusTime.value)
        {
            nextStatusTimer = 0;
            UpdateStatus(nextStatuses[0].predictions, false);
        }
    }

    //Checks if the maximum time for level completion has been met
    //and if true calls Game Over event
    void CheckGameOverTime()
    {
        crossingTimer += Time.deltaTime;

        if (crossingTimer >= levelTime)
        {
            EventManager.InvokeEvent(EventType.GameOver);
        }
    }

    //Checks if the time to spawn another car has been met
    //and calls Spawn Car event to spawn a new car in the game
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
