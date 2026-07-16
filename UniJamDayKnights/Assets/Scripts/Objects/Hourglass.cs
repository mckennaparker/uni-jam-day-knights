using UnityEngine;

public class Hourglass : GateCondition
{
    [Header("Timer")]
    [SerializeField] private float activeDuration = 3f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Hourglass Visual")]
    [SerializeField] private Transform hourglassVisual;
    [SerializeField] private float flipAngle = 180f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Interaction Prompt")]
    [SerializeField] private InteractionPrompt interactionPrompt;

    private float activeTimer;
    private bool playerInRange;
    private float targetZRotation;

    public override bool IsSatisfied => activeTimer > 0f;

    private void Start()
    {
        if (hourglassVisual != null)
        {
            targetZRotation = hourglassVisual.localEulerAngles.z;
        }
    }

    private void Update()
    {
        HandleInteraction();
        UpdateTimer();
        RotateHourglass();
    }

    private void HandleInteraction()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Activate();
        }
    }

    private void UpdateTimer()
    {
        if (activeTimer > 0f)
        {
            activeTimer -= Time.deltaTime;
        }
    }

    public void Activate()
    {
        activeTimer = activeDuration;

        if (hourglassVisual != null)
        {
            targetZRotation += flipAngle;
        }
    }

    private void RotateHourglass()
    {
        if (hourglassVisual == null)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetZRotation);

        hourglassVisual.localRotation =
            Quaternion.RotateTowards(
                hourglassVisual.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetPlayerInRange(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetPlayerInRange(false);
            }
        }
    }
}