using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static Dictionary<EventType, Action<object>> eventDictionary =
        new Dictionary<EventType, Action<object>>();

    //Adds the listener function to the specific event type
    public static void AddListener(EventType eventType, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
            eventDictionary[eventType] = listener;
        else
            eventDictionary[eventType] += listener;
    }

    //Removes the listener function from the specific event type
    public static void RemoveListener(EventType eventType, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventType))
            eventDictionary[eventType] -= listener;
    }

    //Calls all functions registered in the event type passed
    public static void InvokeEvent(EventType eventType, object data = null)
    {
        if (eventDictionary.TryGetValue(eventType, out var thisEvent))
            thisEvent?.Invoke(data);
    }
}
public enum EventType
{
    StatusChanged,
    ResetAPIState,
    LevelAdvance,
    GameStart,
    GameWin,
    GameOver,
    SpawnCar,
    RemoveCar,
    AverageSpeedChange,
    HUDUpdate,
    LevelHUDUpdate,
    APILoaded
}