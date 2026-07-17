using UnityEngine;

public class IndicatorLight : MonoBehaviour
{
    [Header("Day / Night Visuals")]
    [SerializeField] private GameObject brightVisual;
    [SerializeField] private GameObject darkVisual;

    [Header("Animators")]
    [SerializeField] private Animator brightAnimator;
    [SerializeField] private Animator darkAnimator;

    private bool isOn;
    private bool isDarkRoom;

    private static readonly int IsOnHash =
        Animator.StringToHash("IsOn");

    private void Start()
    {
        if (RoomManager.Instance == null)
        {
            Debug.LogError("RoomManager not found.", this);
            return;
        }

        RoomManager.Instance.OnDarkRoomStateChanged += ApplyRoomState;

        ApplyRoomState(RoomManager.Instance.IsDarkRoom);
        ApplyLightState();
    }

    private void OnDestroy()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnDarkRoomStateChanged -= ApplyRoomState;
        }
    }

    public void SetOn(bool value)
    {
        if (isOn == value)
        {
            return;
        }

        isOn = value;
        ApplyLightState();
    }

    private void ApplyRoomState(bool dark)
    {
        isDarkRoom = dark;

        if (brightVisual != null)
        {
            brightVisual.SetActive(!isDarkRoom);
        }

        if (darkVisual != null)
        {
            darkVisual.SetActive(isDarkRoom);
        }

        
        ApplyLightState();
    }

    private void ApplyLightState()
    {
        if (brightAnimator != null)
        {
            brightAnimator.SetBool(IsOnHash, isOn);
        }

        if (darkAnimator != null)
        {
            darkAnimator.SetBool(IsOnHash, isOn);
        }
    }
}