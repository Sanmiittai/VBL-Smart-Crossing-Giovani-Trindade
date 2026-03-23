using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] FloatVariable averageSpeed;

    [SerializeField] float vehicleSpeed;

    bool deactivated = true;

    Vector3 currentDirection = Vector3.right;

    Rigidbody myRigidbody;

    void OnEnable()
    {
        EventManager.AddListener(EventType.AverageSpeedChange, ChangeSpeed);
    }

    void OnDisable()
    {
        EventManager.RemoveListener(EventType.AverageSpeedChange, ChangeSpeed);
    }

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    //Changes the car speed if it's currently activated based on the
    //average speed variable divide by 100 times the base vehicle speed
    public void ChangeSpeed(object data = null)
    {
        if (deactivated) return;
        float myVelocity = averageSpeed.value / 100f * vehicleSpeed;
        Vector3 movementDirection = currentDirection * myVelocity;
        myRigidbody.linearVelocity = movementDirection;
    }

    //Sets the current direction of the movement of the car based
    //on the received direction and activates the car
    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction;
        deactivated = false;
    }

    //Deactivates the car, setting it's velocity to zero and
    //making it unable to gain a new speed by deactivating it
    public void Deactivate()
    {
        myRigidbody.linearVelocity = Vector3.zero;
        deactivated = true;
    }
}
