using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectVisibility : MonoBehaviour
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

        int glyphLayer = LayerMask.NameToLayer("Glyphs");

        // normal sprite objects
        SpriteRenderer[] spriteRenderers =
            group.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.gameObject.layer == glyphLayer)
            {
                spriteRenderer.enabled = true;
                continue;
            }

            spriteRenderer.enabled = visible;
        }

        // tilemap objects
        TilemapRenderer[] tilemapRenderers =
            group.GetComponentsInChildren<TilemapRenderer>(true);

        foreach (TilemapRenderer tilemapRenderer in tilemapRenderers)
        {
            if (tilemapRenderer.gameObject.layer == glyphLayer)
            {
                tilemapRenderer.enabled = true;
                continue;
            }

            tilemapRenderer.enabled = visible;
        }
    }
}