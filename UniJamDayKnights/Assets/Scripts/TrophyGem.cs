using System.Collections;
using UnityEngine;

public class TrophyGem : MonoBehaviour
{
    [Header("Collection Animation")]
    [SerializeField] private float collectDuration = 0.65f;
    [SerializeField] private float riseDistance = 1f;
    [SerializeField] private float rotationAmount = 360f;
    [SerializeField] private float maxScaleMultiplier = 1.4f;

    private SpriteRenderer spriteRenderer;
    private Collider2D gemCollider;

    private Vector3 startPosition;
    private Vector3 startScale;

    private bool collected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gemCollider = GetComponent<Collider2D>();

        startPosition = transform.position;
        startScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected || !collision.CompareTag("Player"))
        {
            return;
        }

        collected = true;
        gemCollider.enabled = false;

        StartCoroutine(CollectAnimation());
    }

    private IEnumerator CollectAnimation()
    {
        float timer = 0f;

        while (timer < collectDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / collectDuration);

            // 위로 이동
            transform.position =
                startPosition + Vector3.up * riseDistance * t;

            // 회전
            transform.rotation =
                Quaternion.Euler(0f, 0f, rotationAmount * t);

            // 처음에는 커지고, 이후 작아짐
            float scaleCurve;

            if (t < 0.35f)
            {
                scaleCurve = Mathf.Lerp(
                    1f,
                    maxScaleMultiplier,
                    t / 0.35f
                );
            }
            else
            {
                scaleCurve = Mathf.Lerp(
                    maxScaleMultiplier,
                    0f,
                    (t - 0.35f) / 0.65f
                );
            }

            transform.localScale =
                startScale * scaleCurve;

            // 점점 투명해짐
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = color;

            yield return null;
        }



        Destroy(gameObject);
    }
}