using UnityEngine;
using UnityEngine.Tilemaps;

public class FakeFloorTilemap : MonoBehaviour
{
    [Header("Fake Floor Tilemaps")]
    [SerializeField] private TilemapRenderer brightTilemapRenderer;
    [SerializeField] private TilemapRenderer darkTilemapRenderer;

    private void Start()
    {
        ApplyRoomState();
    }

    private void ApplyRoomState()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError(
                "RoomManager not found",
                this
            );
            return;
        }

        bool isDarkRoom = RoomManager.Instance.IsDarkRoom;

        if (brightTilemapRenderer != null)
        {
            brightTilemapRenderer.enabled = !isDarkRoom;
        }

        if (darkTilemapRenderer != null)
        {
            darkTilemapRenderer.enabled = isDarkRoom;
        }
    }
}