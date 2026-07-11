using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool IsDead { get; private set; }

    public void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        DisablePlayerMovement();

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RestartRoom();
        }
        else
        {
            Debug.LogError("RoomManager was not found in the scene.");
        }
    }

    private void DisablePlayerMovement()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // player movement script disable
        // GetComponent<PlayerMovement>().enabled = false;
    }
}