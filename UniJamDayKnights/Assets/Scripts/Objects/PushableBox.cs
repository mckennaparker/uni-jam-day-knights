using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private MovingPlatform currentPlatform;

    public bool IsLocked { get; set; }

    public Vector2 RelativeVelocity
    {
        get
        {
            Vector2 platformVelocity =
                currentPlatform != null ? currentPlatform.Velocity : Vector2.zero;

            return rb.linearVelocity - platformVelocity;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponentInParent<MovingPlatform>();

        if (platform == null)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                currentPlatform = platform;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponentInParent<MovingPlatform>();

        if (platform != null && platform == currentPlatform)
            currentPlatform = null;
    }
}