using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    string apiURL = "http://localhost:3001/v1/traffic/status";
    string apiReset = "http://localhost:3001/mockoon-admin/state/purge";

    void OnEnable()
    {
        EventManager.AddListener(EventType.LevelAdvance, NewAPICall);
        EventManager.AddListener(EventType.ResetAPIState, CallResetAPIState);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.LevelAdvance, NewAPICall);
        EventManager.RemoveListener(EventType.ResetAPIState, CallResetAPIState);
    }

    void Start()
    {
        StartCoroutine(ResetAPIState(true));
    }

    void NewAPICall(object data = null)
    {
        StartCoroutine(FetchTrafficStatus());
    }

    void CallResetAPIState(object data = null)
    {
        StartCoroutine(ResetAPIState(false));
    }

    IEnumerator ResetAPIState(bool gameStart)
    {
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(apiReset, ""))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[API] Erros: {request.error}");
            }
            else if (gameStart)
            {
                NewAPICall();
                NewAPICall();
            }
        }
    }

    IEnumerator FetchTrafficStatus()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[API] Error: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                ProcessAPIResponse(jsonResponse);
            }
        }
    }

    void ProcessAPIResponse(string jsonResponse)
    {
        TrafficResponse data = JsonUtility.FromJson<TrafficResponse>(jsonResponse);

        if (data != null)
        {
            EventManager.InvokeEvent(EventType.StatusChanged, data);
        }
        else
        {
            Debug.LogError("Data null");
        }
    }
}

[System.Serializable]
public class Status
{
    public float vehicleDensity;
    public float averageSpeed;
    public string weather;
}

[System.Serializable]
public class PredictedStatusItem 
{
    public int estimated_time;
    public Status predictions;
}

[System.Serializable]
public class TrafficResponse
{
    public Status current_status;
    public PredictedStatusItem[] predicted_status;
}
