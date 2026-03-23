using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] CanvasGroup hud_Group;

    [SerializeField] TMP_Text levelTimer_Text;
    [SerializeField] TMP_Text currentWeather_Text;
    [SerializeField] TMP_Text currentVehicleDensity_Text;
    [SerializeField] TMP_Text currentVehicleAverageSpeed_Text;
    [SerializeField] TMP_Text currentLevel_Text;
    [SerializeField] TMP_Text statusTimer_Text;

    [SerializeField] FloatVariable nextStatusTime;
    [SerializeField] IntVariable currentLevel;

    float levelTimer;
    float statusTimer;

    bool gameStarted;

    void OnEnable()
    {
        EventManager.AddListener(EventType.HUDUpdate, InformationUpdate);
        EventManager.AddListener(EventType.LevelHUDUpdate, UpdateLevel);
        EventManager.AddListener(EventType.GameStart, GameStarted);
        EventManager.AddListener(EventType.GameOver, OnGameOver);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.HUDUpdate, InformationUpdate);
        EventManager.RemoveListener(EventType.LevelHUDUpdate, UpdateLevel);
        EventManager.RemoveListener(EventType.GameStart, GameStarted);
        EventManager.RemoveListener(EventType.GameOver, OnGameOver);
    }

    void GameStarted(object data = null)
    {
        hud_Group.alpha = 1;
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        levelTimer_Text.text = $"Level Timer: {levelTimer:F2}";
        statusTimer_Text.text = $"Status Timer: {statusTimer:F2}";
        levelTimer -= Time.deltaTime;
        statusTimer -= Time.deltaTime;
    }

    void OnGameOver(object data = null)
    {
        gameStarted = false;
        hud_Group.alpha = 0;
    }

    public void UpdateLevel(object data = null)
    {
        levelTimer = (float)data;
        currentLevel_Text.text = $"Current Level: {currentLevel.value}";
    }

    public void InformationUpdate(object data = null)
    {
        Status currentStatus = (Status)data;

        statusTimer = nextStatusTime.value;

        currentWeather_Text.text = $"Current Weather: {currentStatus.weather}";

        currentVehicleDensity_Text.text = $"Current Vehicle Density: {currentStatus.vehicleDensity}";

        currentVehicleAverageSpeed_Text.text = $"Current Vehicle Average Speed: {currentStatus.averageSpeed}";
    }
}
