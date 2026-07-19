using UnityEngine;

public class GateCrushDetector : MonoBehaviour
{
    [Header("Crush Detection")]
    [SerializeField] private Vector2 closingDirection = Vector2.down;
    [SerializeField] private float surfaceCheckDistance = 0.15f;
    [SerializeField] private LayerMask solidLayers;

    private bool isClosing;

    public void SetClosing(bool closing)
    {
        isClosing = closing;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isClosing || !other.CompareTag("Player"))
            return;

        Collider2D playerCollider = other;
        PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();

        if (IsSurfaceBlockingPlayer(playerCollider))
            playerDeath.Die();
    }

    private bool IsSurfaceBlockingPlayer(Collider2D playerCollider)
    {
        Vector2 direction = closingDirection.normalized;
        Bounds playerBounds = playerCollider.bounds;

        float playerExtent = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? playerBounds.extents.x
            : playerBounds.extents.y;

        float checkDistance = playerExtent + surfaceCheckDistance;

        RaycastHit2D hit = Physics2D.Raycast(
            playerBounds.center,
            direction,
            checkDistance,
            solidLayers
        );

        if (hit.collider == null)
            return false;

        if (hit.collider.transform.IsChildOf(transform.root))
            return false;

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, closingDirection.normalized * surfaceCheckDistance);
    }
}