using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Room Settings")]
    [SerializeField] private bool isDarkRoom;

    [Header("Restart Settings")]
    [SerializeField] private float restartDelay = 0.1f;
    public bool IsDarkRoom => isDarkRoom;
    public event Action<bool> OnDarkRoomStateChanged;

    private bool isRestarting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R)) // restart current room by R (optional)
        {
            RestartRoom();
        }

        if (Input.GetKeyDown(KeyCode.T)) // press T to change bright/dark state (debug)
        {
            ToggleLightState();
        }
    }
    public void ToggleLightState()
    {
        SetDarkRoom(!isDarkRoom);
        Debug.Log("Toggle stage");
    }

    public void SetDarkRoom(bool dark)
    {
        if (isDarkRoom == dark)
        {
            return;
        }

        isDarkRoom = dark;
        OnDarkRoomStateChanged?.Invoke(isDarkRoom);
    }
    public void RestartRoom()
    {
        if (isRestarting)
        {
            return;
        }

        isRestarting = true;
        Invoke(nameof(ReloadCurrentScene), restartDelay);
    }
    public void RestartRoom(float delay)
    {
        if (isRestarting)
        {
            return;
        }

        isRestarting = true;
        Invoke(nameof(ReloadCurrentScene), delay);
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}