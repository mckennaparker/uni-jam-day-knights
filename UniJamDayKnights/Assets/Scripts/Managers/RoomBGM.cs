using UnityEngine;

public class RoomBGM : MonoBehaviour
{
    [Header("Room Music")]
    [SerializeField] private AudioClip brightRoomBGM;
    [SerializeField] private AudioClip darkRoomBGM;

    private void Start()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found.", this);
            return;
        }

        RoomManager.Instance.OnDarkRoomStateChanged += ApplyRoomBGM;
        ApplyRoomBGM(RoomManager.Instance.IsDarkRoom);
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= ApplyRoomBGM;
        }
    }

    private void ApplyRoomBGM(bool isDarkRoom)
    {
        AudioClip targetBGM = isDarkRoom
            ? darkRoomBGM
            : brightRoomBGM;

        AudioManager.Instance?.PlayBGM(targetBGM);
    }
}