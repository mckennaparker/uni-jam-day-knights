using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag("Player"))
            return;

        activated = true;
        RoomManager.Instance?.SetCheckpoint(transform.position);

        Debug.Log($"Checkpoint activated: {name}");
    }
}