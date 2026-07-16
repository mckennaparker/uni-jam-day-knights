using System.Collections.Generic;
using UnityEngine;
using System;

public class PressureButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform buttonTop;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer buttonRenderer;
    [SerializeField] private Sprite releasedSprite;
    [SerializeField] private Sprite pressedSprite;

    [Header("Movement")]
    [SerializeField]
    private Vector3 pressedOffset =
        new Vector3(0f, -0.2f, 0f);

    [SerializeField] private float moveSpeed = 8f;

    [Header("Activation")]
    [SerializeField] private LayerMask activatorLayers;

    public bool IsPressed { get; private set; }

    private Vector3 releasedLocalPosition;
    private Vector3 pressedLocalPosition;

    private readonly HashSet<Collider2D> objectsOnButton = new();

    public event Action<PressureButton> OnPressed;
    public event Action<bool> OnPressedChanged;


    private void Awake()
    {
        if (buttonTop == null)
        {
            Debug.LogError("Button Top not assigned.", this);
            enabled = false;
            return;
        }

        releasedLocalPosition = buttonTop.localPosition;
        pressedLocalPosition = releasedLocalPosition + pressedOffset;

        if (buttonRenderer == null)
        {
            buttonRenderer = buttonTop.GetComponent<SpriteRenderer>();
        }
        UpdateButtonSprite();
    }

    private void Update()
    {
        Vector3 targetPosition = IsPressed
            ? pressedLocalPosition
            : releasedLocalPosition;

        buttonTop.localPosition = Vector3.MoveTowards(
            buttonTop.localPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActivator(other.gameObject))
        {
            return;
        }

        objectsOnButton.Add(other);
        UpdatePressedState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsActivator(other.gameObject))
        {
            return;
        }

        objectsOnButton.Remove(other);
        UpdatePressedState();
    }

    private bool IsActivator(GameObject target)
    {
        return (activatorLayers.value & (1 << target.layer)) != 0;
    }

    private void UpdatePressedState()
    {
        bool newPressedState = objectsOnButton.Count > 0;

        if (newPressedState == IsPressed)
        {
            return;
        }

        IsPressed = newPressedState;
        UpdateButtonSprite();
        OnPressedChanged?.Invoke(IsPressed);

        if(IsPressed)
        {
            AudioManager.Instance?.PlayButtonPress();
            OnPressed?.Invoke(this);
        }
            
    }

    private void UpdateButtonSprite()
    {
        if (buttonRenderer == null)
        {
            return;
        }

        buttonRenderer.sprite = IsPressed
            ? pressedSprite
            : releasedSprite;
    }

}