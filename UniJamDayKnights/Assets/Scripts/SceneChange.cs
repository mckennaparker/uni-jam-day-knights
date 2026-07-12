using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    float delay = 2f;
    private int currentSceneIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RespondToDebugKeys();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
}
