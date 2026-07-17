using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    float delay = 0.15f;
    private int currentSceneIndex;
    private bool isTransitioning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isTransitioning)
        {
            return;
        }

        isTransitioning = true;

        AudioManager.Instance?.PlayRoomTrans();

        Invoke(nameof(LoadNextScene), delay);
    }

    private void LoadNextScene()
    {
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            AudioManager.Instance?.PlaylongJingle();    // play game clear sound here for now
            Debug.LogWarning("There is no next scene in Build Settings.");
        }
    }
}
