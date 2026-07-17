using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string finalLevelName = "FinalTrophyRoom";

    private int currentSceneIndex;
    private bool isTransitioning;

    [Header("Final Sequence")]
    [SerializeField] private float finalAutoWalkDuration = 2.5f;
    [SerializeField] private float finalAutoWalkDirection = 1f;

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
        bool isFinalLevel = currentSceneName == finalLevelName;


        if (isFinalLevel)
        {
            PlayerMovement playerMovement =
                collision.GetComponent<PlayerMovement>();

            StartCoroutine(
                FinalSequence(playerMovement, nextSceneIndex)
            );
        }
        else
        {
            FadeManager.Instance.FadeToScene(nextSceneIndex);
        }

    }

    private IEnumerator FinalSequence(
        PlayerMovement playerMovement,
        int nextSceneIndex)
    {
        if (playerMovement != null)
            playerMovement.StartAutoWalk(finalAutoWalkDirection);

        yield return new WaitForSeconds(1f);

        FadeManager.Instance.FadeToFinalScene(nextSceneIndex);
    }
}