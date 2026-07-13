using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Lever Visual")]
    [SerializeField] private Transform leverPivot;
    [SerializeField] private float offAngle = -80f;
    [SerializeField] private float onAngle = 80f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    public bool IsActivated { get; private set; }

    private bool playerInRange;

    private void Start()
    {
        ApplyRotationImmediately();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Toggle();
        }

        RotateLever();
    }

    public void Toggle()
    {
        IsActivated = !IsActivated;
    }

    public void SetActivated(bool activated)
    {
        IsActivated = activated;
    }

    private void RotateLever()
    {
        if (leverPivot == null)
        {
            return;
        }

        float targetAngle = IsActivated ? onAngle : offAngle;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        leverPivot.localRotation = Quaternion.RotateTowards(
            leverPivot.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ApplyRotationImmediately()
    {
        if (leverPivot == null)
        {
            Debug.LogError("Lever Pivot not assigned.", this);
            return;
        }

        float startingAngle = IsActivated ? onAngle : offAngle;
        leverPivot.localRotation =
            Quaternion.Euler(0f, 0f, startingAngle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player entered");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}