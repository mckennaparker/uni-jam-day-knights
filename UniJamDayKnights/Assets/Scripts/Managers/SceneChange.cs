using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string finalLevelName = "FinalTrophyRoom";

    private int currentSceneIndex;
    private bool isTransitioning;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isTransitioning)
            return;

        isTransitioning = true;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("There is no next scene in Build Settings.");
            isTransitioning = false;
            return;
        }

        if (FadeManager.Instance == null)
        {
            Debug.LogError("SceneChange: FadeManager was not found.");
            isTransitioning = false;
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == finalLevelName)
            FadeManager.Instance.FadeToFinalScene(nextSceneIndex);
        else
            FadeManager.Instance.FadeToScene(nextSceneIndex);
    }
}