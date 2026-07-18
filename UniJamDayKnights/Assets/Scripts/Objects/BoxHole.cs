using System.Collections;
using UnityEngine;

public class BoxHole : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private GameObject filledFloor;
    [SerializeField] private float dropDuration = 0.2f;

    private Collider2D holeTrigger;
    private bool isFilled;

    private void Awake()
    {
        holeTrigger = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isFilled)
            return;

        PushableBox box = other.GetComponentInParent<PushableBox>();

        if (box == null || box.IsLocked)
            return;

        Collider2D boxCollider = box.GetComponent<Collider2D>();

        if (boxCollider == null)
            boxCollider = box.GetComponentInChildren<Collider2D>();

        if (boxCollider == null || holeTrigger == null)
            return;

        Bounds boxBounds = boxCollider.bounds;
        Bounds triggerBounds = holeTrigger.bounds;

        bool fullyInsideHorizontally =
            boxBounds.min.x >= triggerBounds.min.x &&
            boxBounds.max.x <= triggerBounds.max.x;

        if (!fullyInsideHorizontally)
            return;

        StartCoroutine(DropBox(box));
    }

    private IEnumerator DropBox(PushableBox box)
    {
        isFilled = true;
        box.IsLocked = true;

        Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();

        if (boxRb == null)
            yield break;

        boxRb.linearVelocity = Vector2.zero;
        boxRb.angularVelocity = 0f;
        boxRb.bodyType = RigidbodyType2D.Kinematic;

        Vector2 startPosition = boxRb.position;

        Vector2 targetPosition = new Vector2(
            boxRb.position.x,
            snapPoint.position.y
        );

        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.fixedDeltaTime;
            float progress = Mathf.Clamp01(elapsed / dropDuration);

            boxRb.MovePosition(
                Vector2.Lerp(startPosition, targetPosition, progress)
            );

            yield return new WaitForFixedUpdate();
        }

        boxRb.position = targetPosition;
        boxRb.linearVelocity = Vector2.zero;

        Collider2D[] boxColliders =
            box.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D boxCollider in boxColliders)
            boxCollider.enabled = false;

        if (filledFloor != null)
            filledFloor.SetActive(true);
    }
}