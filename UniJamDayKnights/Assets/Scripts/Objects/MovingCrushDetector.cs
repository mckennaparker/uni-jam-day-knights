using UnityEngine;

public class MovingCrushDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform movingRoot;

    [Header("Detector Direction")]
    [SerializeField] private Vector2 localCrushDirection = Vector2.right;

    [Header("Detection")]
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private float minimumMoveSpeed = 0.05f;
    [SerializeField] private float obstacleCheckDistance = 0.2f;
    [SerializeField] private float directionThreshold = 0.5f;
    [SerializeField] private float teleportThreshold = 3f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs;
    [SerializeField] private bool drawDebugRay = true;

    [SerializeField] private AudioClip deathSound;

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

        Vector2 crushDirection = GetWorldCrushDirection();
        Vector2 moveDirection = movementVelocity.normalized;

        float directionMatch = Vector2.Dot(moveDirection, crushDirection);

        if (directionMatch < directionThreshold)
            return;

        if (!HasBlockingSurface(other, crushDirection))
            return;

        playerKilled = true;

        if (showDebugLogs)
            Debug.Log($"Player crushed by {movingRoot.name} toward {crushDirection}", this);

        PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
        AudioManager.Instance?.PlaySfx(deathSound);
        playerDeath.Die();
    }

    private Vector2 GetWorldCrushDirection()
    {
        Vector3 worldDirection = transform.TransformDirection(localCrushDirection);
        return ((Vector2)worldDirection).normalized;
    }

    private bool HasBlockingSurface(Collider2D playerCollider, Vector2 direction)
    {
        Bounds bounds = playerCollider.bounds;

        float playerExtent = Mathf.Abs(direction.x) * bounds.extents.x +
                             Mathf.Abs(direction.y) * bounds.extents.y;

        float distance = playerExtent + obstacleCheckDistance;
        RaycastHit2D[] hits = Physics2D.RaycastAll(bounds.center, direction, distance, solidLayers);

        bool foundSurface = false;

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

            foundSurface = true;

            if (showDebugLogs)
                Debug.Log($"Blocking surface found: {hit.collider.name}", this);

            break;
        }

        if (drawDebugRay)
        {
            Color rayColor = foundSurface ? Color.red : Color.green;
            Debug.DrawRay(bounds.center, direction * distance, rayColor, Time.fixedDeltaTime);
        }

        return foundSurface;
    }

    private bool IsPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return true;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
            return true;

        return false;
    }
}