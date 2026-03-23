using UnityEngine;

public class CarLimiter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Car otherCar = other.GetComponent<Car>();
        if (otherCar != null)
            EventManager.InvokeEvent(EventType.RemoveCar, otherCar);
    }
}
