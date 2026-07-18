using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 previousPosition;
    private Rigidbody2D rb;
    private float elapsedTime;

    public Vector2 Velocity { get; private set; }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveDistance = 2f;

    [SerializeField] private bool isHorizontal;
    [SerializeField] private bool leftFirst;
    [SerializeField] private Transform glyphs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPos = rb.position;
        previousPosition = rb.position;
        elapsedTime = 0f;

        if (glyphs != null)
        {
            glyphs.localRotation = isHorizontal ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;

        float offset = Mathf.Sin(elapsedTime * moveSpeed) * moveDistance;

        Vector2 targetPosition = startPos;

        if (isHorizontal)
        {
            targetPosition.x += leftFirst ? -offset : offset;
        }
        else
        {
            targetPosition.y += leftFirst ? -offset : offset;
        }

        Velocity = (targetPosition - previousPosition) / Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        previousPosition = targetPosition;
    }
}