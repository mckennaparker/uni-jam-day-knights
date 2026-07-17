using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private GameObject mainMenuPanel;

    public AudioMixer audioMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance?.PlayMenuBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        // Load the first level of the game asynchronously
        SceneManager.LoadScene("Scenes/Levels/Level1");
    }

    public void ViewOptions()
    {
        // Load the options scene
        SceneManager.LoadScene("Scenes/UI/Options");
    }

    public void ViewCredits()
    {
        // Load the credits scene
        SceneManager.LoadScene("Scenes/UI/Credits");
    }

    public void QuitGame()
    {
        // Exit the game
        Debug.Log("Quitting game..."); // helpful since this won't fire in the Editor
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
