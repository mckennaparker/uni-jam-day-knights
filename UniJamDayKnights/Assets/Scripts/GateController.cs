using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Condition")]
    [SerializeField] private GateCondition openCondition;

    [Header("Movement")]
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float moveSpeed = 3f;

    [Header("Behavior")]
    [SerializeField] private bool stayOpenOnceActivated;

    public bool IsOpen { get; private set; }

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        if (openCondition == null)
        {
            return;
        }

        bool shouldOpen = openCondition.IsSatisfied;

        if (stayOpenOnceActivated && IsOpen)
        {
            shouldOpen = true;
        }

        IsOpen = shouldOpen;

        Vector3 targetPosition = IsOpen
            ? openPosition
            : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}