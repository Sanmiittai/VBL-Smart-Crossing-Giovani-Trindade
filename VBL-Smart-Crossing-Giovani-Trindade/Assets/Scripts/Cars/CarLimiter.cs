using UnityEngine;

public class CarLimiter : MonoBehaviour
{
    //Checks if a car passed the car limiter and calls the RemoveCar event,
    //deactivating the car and putting it back in the pull
    void OnTriggerEnter(Collider other)
    {
        Car otherCar = other.GetComponent<Car>();
        if (otherCar != null)
            EventManager.InvokeEvent(EventType.RemoveCar, otherCar);
    }
}
