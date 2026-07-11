using UnityEngine;

public class RoomVisibility : MonoBehaviour
{
    [Header("Object Groups")]
    [SerializeField] private GameObject objectsToHide;
    [SerializeField] private GameObject glyphs;

    private void Start()
    {
        ApplyVisibility();
    }

    [ContextMenu("Apply Visibility")]
    public void ApplyVisibility()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found.", this);
            return;
        }

        bool isDarkRoom = RoomManager.Instance.IsDarkRoom;

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