using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    [Header("Prompt Visual")]
    [SerializeField] private GameObject promptVisual;

    private bool playerInRange;

    private void Start()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found.", this);
            return;
        }

        RoomManager.Instance.OnDarkRoomStateChanged += HandleRoomStateChanged;

        RefreshPrompt();
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= HandleRoomStateChanged;
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        playerInRange = inRange;
        RefreshPrompt();
    }

    private void HandleRoomStateChanged(bool isDarkRoom)
    {
        RefreshPrompt();
    }

    private void RefreshPrompt()
    {
        if (promptVisual == null)
        {
            return;
        }

        bool isBrightRoom =
            RoomManager.Instance != null &&
            !RoomManager.Instance.IsDarkRoom;

        promptVisual.SetActive(playerInRange && isBrightRoom);
    }
}