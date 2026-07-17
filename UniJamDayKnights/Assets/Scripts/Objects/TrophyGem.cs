using System.Collections;
using UnityEngine;

public class TrophyGem : MonoBehaviour
{
    [Header("Collection Target")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Spiral Animation")]
    [SerializeField] private float collectDuration = 1.5f;
    [SerializeField] private float spiralTurns = 2f;
    [SerializeField] private float startingRadius = 1.5f;
    [SerializeField] private float endingRadius = 0f;
    [SerializeField] private float spinDirection = 1f;

    [Header("Disappear")]
    [SerializeField] private float shrinkStartTime = 0.75f;

    [Header("Idle Floating")]
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Optional")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private Behaviour componentToEnable;

    private SpriteRenderer spriteRenderer;
    private Collider2D gemCollider;

    private Transform player;

    private Vector3 originalScale;
    private Vector3 idleStartPosition;

    private bool collected;
    private float floatPhase;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gemCollider = GetComponent<Collider2D>();

        originalScale = transform.localScale;
        idleStartPosition = transform.position;

        floatPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        if (collected)
            return;

        float yOffset = Mathf.Sin(Time.time * floatSpeed + floatPhase) * floatHeight;

        transform.position = idleStartPosition + Vector3.up * yOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected || !collision.CompareTag("Player"))
            return;

        collected = true;
        player = collision.transform;

        gemCollider.enabled = false;

        StartCoroutine(CollectSpiral());
    }

    private IEnumerator CollectSpiral()
    {
        float elapsed = 0f;

        Vector3 startPosition = transform.position;

        Vector3 targetPosition = player.position + targetOffset;

        Vector2 startOffset = startPosition - targetPosition;

        float startAngle = Mathf.Atan2(startOffset.y, startOffset.x);

        float actualStartingRadius = startOffset.magnitude;

        if (actualStartingRadius < 0.1f)
            actualStartingRadius = startingRadius;

        while (elapsed < collectDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / collectDuration);

            // 회전은 일정한 속도
            float rotationT = t;

            // 마지막으로 갈수록 중심으로 빨려감
            float radiusT = t * t;

            targetPosition = player.position + targetOffset;

            float radius = Mathf.Lerp(actualStartingRadius, endingRadius, radiusT);

            float angle = startAngle +
                          spiralTurns *
                          Mathf.PI *
                          2f *
                          rotationT *
                          spinDirection;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0f) * radius;

            transform.position = targetPosition + offset;

            transform.localScale = originalScale * GetScaleMultiplier(t);

            Color color = spriteRenderer.color;

            if (t > shrinkStartTime)
            {
                float fadeT = Mathf.InverseLerp(shrinkStartTime, 1f, t);
                color.a = Mathf.Lerp(1f, 0f, fadeT);
            }

            spriteRenderer.color = color;

            yield return null;
        }

        UnlockExit();

        Destroy(gameObject);
    }

    private float GetScaleMultiplier(float t)
    {
        if (t < shrinkStartTime)
            return 1f;

        float shrinkT = Mathf.InverseLerp(shrinkStartTime, 1f, t);

        return Mathf.Lerp(1f, 0f, shrinkT);
    }

    private void UnlockExit()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (componentToEnable != null)
            componentToEnable.enabled = true;
    }
}