using UnityEngine;

public class MovingCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform movingRoot;

    [Header("Detection")]
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private float minimumMoveSpeed = 0.05f;
    [SerializeField] private float obstacleCheckDistance = 0.2f;
    [SerializeField] private float teleportThreshold = 3f;

    [SerializeField] private AudioClip deathSound;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs;
    [SerializeField] private bool drawDebugRay = true;

    private Vector3 previousPosition;
    private Vector2 movementVelocity;
    private bool playerKilled;

    private void Awake()
    {
        if (movingRoot == null)
            movingRoot = transform.parent;
    }

    private void OnEnable()
    {
        if (movingRoot != null)
            previousPosition = movingRoot.position;

        movementVelocity = Vector2.zero;
        playerKilled = false;
    }

    private void FixedUpdate()
    {
        if (movingRoot == null)
            return;

        Vector3 currentPosition = movingRoot.position;
        Vector3 movement = currentPosition - previousPosition;

        if (movement.magnitude > teleportThreshold)
            movementVelocity = Vector2.zero;
        else
            movementVelocity = movement / Time.fixedDeltaTime;

        previousPosition = currentPosition;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (playerKilled || !IsPlayer(other))
            return;

        if (movementVelocity.magnitude < minimumMoveSpeed)
            return;

        Vector2 moveDirection = movementVelocity.normalized;

        if (!IsPlayerInFront(other, moveDirection))
            return;

        if (!HasBlockingSurface(other, moveDirection))
            return;

        playerKilled = true;

        if (showDebugLogs)
            Debug.Log($"Player crushed by {movingRoot.name}", this);

        PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
        AudioManager.Instance?.PlaySfx(deathSound);
        playerDeath.Die();
    }

    private bool IsPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return true;

        if (other.attachedRigidbody != null &&
            other.attachedRigidbody.CompareTag("Player"))
            return true;

        return false;
    }

    private bool IsPlayerInFront(Collider2D playerCollider, Vector2 moveDirection)
    {
        Vector2 moverCenter = transform.position;
        Vector2 playerCenter = playerCollider.bounds.center;
        Vector2 directionToPlayer = playerCenter - moverCenter;

        float dot = Vector2.Dot(directionToPlayer, moveDirection);

        if (showDebugLogs)
            Debug.Log($"Player front dot: {dot}", this);

        return dot > 0f;
    }

    private bool HasBlockingSurface(Collider2D playerCollider, Vector2 moveDirection)
    {
        Bounds playerBounds = playerCollider.bounds;

        float playerExtent =
            Mathf.Abs(moveDirection.x) * playerBounds.extents.x +
            Mathf.Abs(moveDirection.y) * playerBounds.extents.y;

        float checkDistance = playerExtent + obstacleCheckDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            playerBounds.center,
            moveDirection,
            checkDistance,
            solidLayers
        );

        if (drawDebugRay)
        {
            Debug.DrawRay(
                playerBounds.center,
                moveDirection * checkDistance,
                hits.Length > 0 ? Color.red : Color.green,
                Time.fixedDeltaTime
            );
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null)
                continue;

            if (hit.collider == playerCollider)
                continue;

            if (movingRoot != null &&
                (hit.collider.transform == movingRoot ||
                 hit.collider.transform.IsChildOf(movingRoot)))
            {
                continue;
            }

            if (showDebugLogs)
                Debug.Log($"Blocking surface: {hit.collider.name}", this);

            return true;
        }

        return false;
    }
}