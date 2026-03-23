using UnityEngine;

public class LevelFinishLine : MonoBehaviour
{
    [SerializeField] BoxCollider levelBarrier;

    bool isCrossed;

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
