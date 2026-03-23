using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerBaseSpeed;
    [SerializeField] FloatVariable playerSpeedMultiplier;

    [SerializeField] InputActionReference moveReference;

    Rigidbody myRigidbody;
    Vector2 moveDirection;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = moveReference.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float playerFinalSpeed = playerBaseSpeed * playerSpeedMultiplier.value;
        myRigidbody.linearVelocity = new Vector3(moveDirection.x * playerFinalSpeed, 0, moveDirection.y * playerFinalSpeed);
    }
}
