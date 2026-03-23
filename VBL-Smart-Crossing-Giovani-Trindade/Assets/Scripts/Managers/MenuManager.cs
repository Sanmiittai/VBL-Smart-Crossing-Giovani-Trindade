using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup start_Group;
    [SerializeField] CanvasGroup gameOver_Group;
    [SerializeField] CanvasGroup gameWin_Group;
    [SerializeField] TMP_Text startText;
    bool gameStarted;
    bool apiLoaded;

    void OnEnable()
    {
        EventManager.AddListener(EventType.APILoaded, OnAPILoad);
        EventManager.AddListener(EventType.GameOver, OnGameOver);
        EventManager.AddListener(EventType.GameWin, OnGameWin);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.APILoaded, OnAPILoad);
        EventManager.RemoveListener(EventType.GameOver, OnGameOver);
        EventManager.RemoveListener(EventType.GameWin, OnGameWin);
    }

    void OnAPILoad(object data = null)
    {
        apiLoaded = true;
        startText.text = "Press any key to start simulation...";
    }

    void OnGameWin(object data = null)
    {
        gameWin_Group.alpha = 1;
        gameWin_Group.interactable = true;
        gameWin_Group.blocksRaycasts = true;
    }

    void OnGameOver(object data = null)
    {
        gameOver_Group.alpha = 1;
        gameOver_Group.interactable = true;
        gameOver_Group.blocksRaycasts = true;
    }

    public void RestartSimulation()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    void Update()
    {
        if (apiLoaded && !gameStarted && Input.anyKeyDown)
        {
            EventManager.InvokeEvent(EventType.GameStart);
            gameStarted = true;
            start_Group.alpha = 0;
        }
    }
}
