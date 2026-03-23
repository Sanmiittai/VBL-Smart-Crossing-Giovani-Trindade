using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    bool isDead;

    //Checks if the player collided with a car and triggers the Game Over
    void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Damage"))
        {
            EventManager.InvokeEvent(EventType.GameOver);
            isDead = true;
        }
    }
}
