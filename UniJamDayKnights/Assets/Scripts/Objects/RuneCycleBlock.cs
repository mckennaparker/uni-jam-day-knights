using UnityEngine;

public class RuneCycleBlock : MonoBehaviour
{
    [SerializeField] private Sprite[] numberRunes;
    [SerializeField] private SpriteRenderer numberRenderer;
    [SerializeField] private int currentIndex;

    private void Start()
    {
        UpdateRuneVisual();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            bool playerHitFromBelow = contact.normal.y > 0.5f;

            if (playerHitFromBelow)
            {
                AudioManager.Instance?.PlayInteraction();
                AdvanceRune();
                break;
            }
        }
    }

    private void AdvanceRune()
    {
        if (numberRunes == null || numberRunes.Length == 0)
        {
            return;
        }

        currentIndex =
            (currentIndex + 1) % numberRunes.Length;

        UpdateRuneVisual();
    }

    private void UpdateRuneVisual()
    {
        if (numberRenderer == null ||
            numberRunes == null ||
            numberRunes.Length == 0)
        {
            return;
        }

        numberRenderer.sprite = numberRunes[currentIndex];
    }

    public int CurrentValue => currentIndex + 1;
}