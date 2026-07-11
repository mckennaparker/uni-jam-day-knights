using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FakeFloor : MonoBehaviour
{
    [SerializeField] private Sprite brightSprite;
    [SerializeField] private Sprite darkSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found", this);
            return;
        }

        spriteRenderer.sprite = RoomManager.Instance.IsDarkRoom
            ? darkSprite
            : brightSprite;
    }
}