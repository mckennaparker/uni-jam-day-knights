using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Room Settings")]
    [SerializeField] private bool isDarkRoom;

    [Header("Restart Settings")]
    [SerializeField] private float restartDelay = 0.25f;
    public bool IsDarkRoom => isDarkRoom;

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

    private void Update() // restart current room by R (optional)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartRoom();
        }
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

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}