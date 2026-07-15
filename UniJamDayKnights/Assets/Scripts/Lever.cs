using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Lever Visual")]
    [SerializeField] private Transform lightIndicator;

    [SerializeField]
    private Vector3 offLocalPosition =
        new Vector3(0f, -0.2f, 0f);

    [SerializeField]
    private Vector3 onLocalPosition =
        new Vector3(0f, 0.2f, 0f);

    [SerializeField] private float visualMoveSpeed = 2f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Starting State")]
    [SerializeField] private bool startActivated;

    public bool IsActivated { get; private set; }

    public event Action<Lever> OnActivated;
    public event Action<Lever, bool> OnStateChanged;

    private bool playerInRange;

    private void Awake()
    {
        IsActivated = startActivated;
    }

    private void Start()
    {
        ApplyVisualImmediately();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Toggle();
        }

        MoveIndicator();
    }

    public void Toggle()
    {
        SetActivated(!IsActivated);
    }

    public void SetActivated(bool activated)
    {
        if (IsActivated == activated)
        {
            return;
        }

        IsActivated = activated;

        OnStateChanged?.Invoke(this, IsActivated);

        if (IsActivated)
        {
            OnActivated?.Invoke(this);
        }
    }

    private void MoveIndicator()
    {
        if (lightIndicator == null)
        {
            return;
        }

        Vector3 targetPosition = IsActivated
            ? onLocalPosition
            : offLocalPosition;

        lightIndicator.localPosition = Vector3.MoveTowards(
            lightIndicator.localPosition,
            targetPosition,
            visualMoveSpeed * Time.deltaTime
        );
    }

    private void ApplyVisualImmediately()
    {
        if (lightIndicator == null)
        {
            Debug.LogError("Light Indicator is not assigned.", this);
            return;
        }

        lightIndicator.localPosition = IsActivated
            ? onLocalPosition
            : offLocalPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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