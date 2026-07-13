using UnityEngine;

public class RoomVisibility : MonoBehaviour
{
    [SerializeField] private GameObject objectsToHide;
    [SerializeField] private GameObject glyphs;

    private void Start()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError(
                "RoomManager not found.",
                this
            );
            return;
        }

        RoomManager.Instance.OnDarkRoomStateChanged += ApplyVisibility;

        ApplyVisibility(RoomManager.Instance.IsDarkRoom);
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= ApplyVisibility;
        }
    }

    private void ApplyVisibility(bool isDarkRoom)
    {
        SetRenderersVisible(objectsToHide, !isDarkRoom);

        if (glyphs != null)
        {
            glyphs.SetActive(true);
        }
    }

    private void SetRenderersVisible(GameObject group, bool visible)
    {
        if (group == null)
        {
            return;
        }

        SpriteRenderer[] renderers =
            group.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.enabled = visible;
        }
    }
}