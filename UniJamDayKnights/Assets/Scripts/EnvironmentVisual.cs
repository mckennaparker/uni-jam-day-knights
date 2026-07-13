using UnityEngine;

public class EnvironmentalVisual : MonoBehaviour
{
    [Header("Visual Groups")]
    [SerializeField] private GameObject brightVisuals;
    [SerializeField] private GameObject darkVisuals;

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

        RoomManager.Instance.OnDarkRoomStateChanged += ApplyState;

        ApplyState(RoomManager.Instance.IsDarkRoom);
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= ApplyState;
        }
    }

    private void ApplyState(bool isDarkRoom)
    {
        if (brightVisuals != null)
        {
            brightVisuals.SetActive(!isDarkRoom);
        }

        if (darkVisuals != null)
        {
            darkVisuals.SetActive(isDarkRoom);
        }
    }
}