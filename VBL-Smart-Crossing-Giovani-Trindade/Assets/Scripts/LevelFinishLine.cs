using UnityEngine;

public class LevelFinishLine : MonoBehaviour
{
    [SerializeField] BoxCollider levelBarrier;

    bool isCrossed;

    //Checks if player crossed the finish line to advance to the next level
    //and block the player from going to the previous level
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCrossed)
        {
            isCrossed = true;
            levelBarrier.enabled = true;
            EventManager.InvokeEvent(EventType.LevelAdvance);
        }
    }
}
