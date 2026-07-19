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

    private static Vector3 checkpointPosition;
    private static string checkpointSceneName;
    private static bool hasCheckpoint;

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
    public void RestartRoom(float delayBeforeFade = 0f)
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeRestartScene(delayBeforeFade);
        }
        else
        {
            Debug.LogWarning(
                "FadeManager was not found. Restarting without fade."
            );

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        checkpointSceneName = SceneManager.GetActiveScene().name;
        hasCheckpoint = true;

        Debug.Log($"Checkpoint saved at {checkpointPosition}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!hasCheckpoint || scene.name != checkpointSceneName)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player was not found after scene reload.");
            return;
        }

        player.transform.position = checkpointPosition;

        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        if (playerRigidbody != null)
            playerRigidbody.linearVelocity = Vector2.zero;

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;

            cameraPosition.x = checkpointPosition.x;
            cameraPosition.y = checkpointPosition.y;

            mainCamera.transform.position = cameraPosition;
        }

        Debug.Log($"Player and camera respawned at {checkpointPosition}");
    }
}