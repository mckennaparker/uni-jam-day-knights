using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlyphFloatAndPulse : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatDistance = 0.05f;
    [SerializeField] private float floatSpeed = 1f;

    [Header("Fading")]
    [SerializeField, Range(0f, 1f)] private float minAlpha = 0.75f;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 1f;
    [SerializeField] private float pulseSpeed = 1.2f;

    [Header("Variation")]
    [SerializeField] private bool randomizeStart = true;

    private SpriteRenderer spriteRenderer;
    private Vector3 startLocalPosition;
    private Color originalColor;
    private float timeOffset;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startLocalPosition = transform.localPosition;
        originalColor = spriteRenderer.color;

        if (randomizeStart)
        {
            timeOffset = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    private void Update()
    {
        float floatWave =
            Mathf.Sin(Time.time * floatSpeed + timeOffset);

        transform.localPosition =
            startLocalPosition +
            Vector3.up * floatWave * floatDistance;

        float alphaWave =
            (Mathf.Sin(Time.time * pulseSpeed + timeOffset) + 1f) * 0.5f;

        Color newColor = originalColor;
        newColor.a = Mathf.Lerp(minAlpha, maxAlpha, alphaWave);
        spriteRenderer.color = newColor;
    }
}