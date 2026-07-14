using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 startPos, movePos;

    public float moveSpeed;
    public float moveDistance;

    public bool isHorizontal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal)
        {
            movePos.x = startPos.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector2(movePos.x, transform.position.y);
        }
        else
        {
            movePos.y = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector2(transform.position.x, movePos.y);
        }
    }
}
