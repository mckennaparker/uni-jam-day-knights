using UnityEngine;

public class ShowGlyphWhenStopped : MonoBehaviour
{
    [Header("Target Box")]
    [SerializeField] private Rigidbody2D targetRigidbody;

    [Header("Stopped Detection")]
    [SerializeField] private float speedThreshold = 0.05f;
    [SerializeField] private float stoppedDelay = 0.1f;

    private SpriteRenderer[] glyphRenderers;
    private float stoppedTimer;
    private bool isVisible;

    private void Awake()
    {
        glyphRenderers = GetComponentsInChildren<SpriteRenderer>(true);
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
            glyphRenderer.enabled = visible;
        }
    }
}