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
        EventManager.AddListener(EventType.GameWin, OnHideHud);
        EventManager.AddListener(EventType.GameStart, GameStarted);
        EventManager.AddListener(EventType.GameOver, OnHideHud);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.HUDUpdate, InformationUpdate);
        EventManager.RemoveListener(EventType.LevelHUDUpdate, UpdateLevel);
        EventManager.RemoveListener(EventType.GameWin, OnHideHud);
        EventManager.RemoveListener(EventType.GameStart, GameStarted);
        EventManager.RemoveListener(EventType.GameOver, OnHideHud);
    }

    //Shows the HUD
    void GameStarted(object data = null)
    {
        hud_Group.alpha = 1;
        gameStarted = true;
    }
    
    //Hides the HUD on Game Over and Game Win
    void OnHideHud(object data = null)
    {
        gameStarted = false;
        hud_Group.alpha = 0;
    }

    //Constantly updates the remaining level timer and status timer
    void Update()
    {
        if (!gameStarted) return;

        levelTimer_Text.text = $"Level Timer: {levelTimer:F2}";
        statusTimer_Text.text = $"Status Timer: {statusTimer:F2}";
        levelTimer -= Time.deltaTime;
        statusTimer -= Time.deltaTime;
    }


    //Updates the current level information on the HUD
    public void UpdateLevel(object data = null)
    {
        levelTimer = (float)data;
        currentLevel_Text.text = $"Current Level: {currentLevel.value}";
    }

    //Updates the variables information on the HUD
    public void InformationUpdate(object data = null)
    {
        Status currentStatus = (Status)data;

        statusTimer = nextStatusTime.value;

        currentWeather_Text.text = $"Current Weather: {currentStatus.weather}";

        currentVehicleDensity_Text.text = $"Current Vehicle Density: {currentStatus.vehicleDensity}";

        currentVehicleAverageSpeed_Text.text = $"Current Vehicle Average Speed: {currentStatus.averageSpeed}";
    }
}
