using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 startPos;
    private Rigidbody2D rb;

    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float moveDistance = 2f;

    [SerializeField] public bool isHorizontal;
    [SerializeField] public bool leftFirst; // Equivalent to downFirst for vertical platforms
    [SerializeField] private Transform glyphs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = rb.position;

        if (glyphs != null)
        {
            glyphs.localRotation = isHorizontal
                ? Quaternion.Euler(0f, 0f, 90f)
                : Quaternion.identity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.fixedTime * moveSpeed) * moveDistance;
        Vector2 targetPosition = startPos;

        if (isHorizontal)
        {
            if (leftFirst)
            {
                targetPosition.x -= offset;
            }
            else
            {
                targetPosition.x += offset;
            }
        }
        else
        {
            if (leftFirst)
            {
                targetPosition.y -= offset;
            }
            else
            {
                targetPosition.y += offset;
            }
        }

        rb.MovePosition(targetPosition);
    }
}
