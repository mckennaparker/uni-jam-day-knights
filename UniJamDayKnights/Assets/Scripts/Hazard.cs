using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();

        if (playerDeath != null)
        {
            AudioManager.Instance?.PlaySfx(deathSound);
            playerDeath.Die();
        }
    }
}