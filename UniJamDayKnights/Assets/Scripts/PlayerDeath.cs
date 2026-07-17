using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float deathRestartDelay = 2f;
    [SerializeField] private ParticleSystem deathParticlesPrefab;

    public bool IsDead { get; private set; }

    public void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        ParticleSystem particles = Instantiate(
            deathParticlesPrefab,
            transform.position,
            Quaternion.identity
        );

        particles.gameObject.SetActive(true);
        particles.Play(true);


        DisablePlayerMovement();

        if (RoomManager.Instance != null)
            RoomManager.Instance.RestartRoom(deathRestartDelay);
        else
            Debug.LogError("RoomManager was not found in the scene.");
    }

    private void DisablePlayerMovement()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}