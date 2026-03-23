using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static Dictionary<EventType, Action<object>> eventDictionary =
        new Dictionary<EventType, Action<object>>();

    public static void AddListener(EventType eventType, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
            eventDictionary[eventType] = listener;
        else
            eventDictionary[eventType] += listener;
    }

    public static void RemoveListener(EventType eventType, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventType))
            eventDictionary[eventType] -= listener;
    }

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
    GameOver,
    SpawnCar
}