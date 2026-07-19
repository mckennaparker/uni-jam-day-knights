using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FakeFloorFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap fakeFloorTilemap;
    [SerializeField] private Collider2D triggerCollider;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("Behavior")]
    [SerializeField] private bool disableAfterFade = true;

    private bool activated;

    private Coroutine fadeCoroutine;
    private bool playerInside;
    private Color originalColor;

    private void Awake()
    {
        if (fakeFloorTilemap == null)
        {
            fakeFloorTilemap =
                GetComponentInChildren<Tilemap>();
        }

        if (triggerCollider == null)
        {
            triggerCollider =
                GetComponentInChildren<Collider2D>();
        }

        if (fakeFloorTilemap != null)
        {
            originalColor = fakeFloorTilemap.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = true;

        StartFade(false);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = false;

        StartFade(true);
    }
    private void StartFade(bool fadeIn)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(
            FadeRoutine(fadeIn)
        );
    }
    private IEnumerator FadeRoutine(bool fadeIn)
    {
        Color start = fakeFloorTilemap.color;
        Color end;

        if (fadeIn)
        {
            end = originalColor;
        }
        else
        {
            end = Color.black;
            end.a = 0f;
        }

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / fadeDuration
            );

            fakeFloorTilemap.color =
                Color.Lerp(start, end, t);

            yield return null;
        }

        fakeFloorTilemap.color = end;
        fadeCoroutine = null;
    }
}