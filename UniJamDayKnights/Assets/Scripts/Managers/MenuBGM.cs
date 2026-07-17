using UnityEngine;

public class MenuBGM : MonoBehaviour
{
    [Header("Room Music")]
    [SerializeField] private AudioClip menuBGM;

    private void Start()
    {
        AudioManager.Instance?.PlayMenuBGM();
    }
}
