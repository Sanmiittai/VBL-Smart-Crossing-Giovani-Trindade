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

    public void ChangeSpeed(object data = null)
    {
        if (deactivated) return;
        float myVelocity = averageSpeed.value / 100f * vehicleSpeed;
        Vector3 movementDirection = currentDirection * myVelocity;
        myRigidbody.linearVelocity = movementDirection;
    }

    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction;
        deactivated = false;
    }

    public void Deactivate()
    {
        myRigidbody.linearVelocity = Vector3.zero;
        deactivated = true;
    }
}
