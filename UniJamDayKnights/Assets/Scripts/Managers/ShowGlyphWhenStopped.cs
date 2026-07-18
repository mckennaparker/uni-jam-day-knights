using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShowGlyphWhenStopped : MonoBehaviour
{
    [Header("Target Box")]
    [SerializeField] private Rigidbody2D targetRigidbody;

    [Header("Stopped Detection")]
    [SerializeField] private float speedThreshold = 0.05f;
    [SerializeField] private float stoppedDelay = 0.1f;

    [Header("Glyph Objects")]
    private SpriteRenderer[] glyphRenderers;
    private Light2D[] glyphLights;

    private float stoppedTimer;
    private bool isVisible;

    private void Awake()
    {
        glyphRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        glyphLights = GetComponentsInChildren<Light2D>(true);
    }

    private void Start()
    {
        SetGlyphVisible(false);
    }

    private void Update()
    {
        if (targetRigidbody == null)
        {
            return;
        }

        // use only horizontal move
        bool isStopped =
            Mathf.Abs(targetRigidbody.linearVelocity.x) <= speedThreshold;

        if (isStopped)
        {
            stoppedTimer += Time.deltaTime;

            if (stoppedTimer >= stoppedDelay)
            {
                SetGlyphVisible(true);
            }
        }
        else
        {
            stoppedTimer = 0f;
            SetGlyphVisible(false);
        }
    }

    private void SetGlyphVisible(bool visible)
    {
        if (isVisible == visible)
        {
            return;
        }

        isVisible = visible;

        foreach (SpriteRenderer glyphRenderer in glyphRenderers)
        {
            if (glyphRenderer != null)
            {
                glyphRenderer.enabled = visible;
            }
        }

        foreach (Light2D glyphLight in glyphLights)
        {
            if (glyphLight != null)
            {
                glyphLight.enabled = visible;
            }
        }
    }
}