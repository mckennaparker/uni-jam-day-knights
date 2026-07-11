using UnityEngine;

public class RoomVisibility : MonoBehaviour
{
    [Header("Room Type")]
    [SerializeField] private bool isDarkRoom;

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