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

    [SerializeField] private Animator animator;

    [Header("Glyphs")]
    [SerializeField] private GameObject[] glyphs;

    public bool IsOpen { get; private set; }

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found.", this);
            return;
        }

        RoomManager.Instance.OnDarkRoomStateChanged += ApplyRoomState;
        ApplyRoomState(RoomManager.Instance.IsDarkRoom);
    }
    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= ApplyRoomState;
        }
    }

    private void ApplyRoomState(bool isDarkRoom)
    {
        animator.SetBool("IsDark", isDarkRoom);
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

        if (shouldOpen != IsOpen)
        {
            IsOpen = shouldOpen;
            SetGlyphsVisible(!IsOpen);
            AudioManager.Instance?.PlayGate();
        }

        Vector3 targetPosition = IsOpen
            ? openPosition
            : closedPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void SetGlyphsVisible(bool visible)
    {
        if (glyphs == null)
        {
            return;
        }

        foreach (GameObject glyph in glyphs)
        {
            if (glyph != null)
            {
                glyph.SetActive(visible);
            }
        }
    }
}