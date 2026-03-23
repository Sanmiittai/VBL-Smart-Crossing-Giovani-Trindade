using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    bool isDead;
    void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Damage"))
        {
            EventManager.InvokeEvent(EventType.GameOver);
            isDead = true;
        }
    }
}
