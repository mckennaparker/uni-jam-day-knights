using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 startPos, movePos;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float moveDistance;

    public bool isHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.fixedTime * moveSpeed) * moveDistance;
        Vector2 targetPosition = startPos;

        if (isHorizontal)
        {
            targetPosition.x += offset;
        }
        else
        {
            targetPosition.y += offset;
        }

        rb.MovePosition(targetPosition);
    }
}
