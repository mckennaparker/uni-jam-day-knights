using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip menuBGM;

    [Header("Basic SFX")]
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip interactionClip;
    [SerializeField] private AudioClip leverClip;
    [SerializeField] private AudioClip gateClip;
    [SerializeField] private AudioClip roomTransClip;

    [SerializeField] private AudioClip[] footstepSounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonPress()
    {
        PlaySfx(buttonPressClip);
    }

    public void PlayInteraction()
    {
        PlaySfx(interactionClip);
    }

    public void PlayLever()
    {
        PlaySfx(leverClip);
    }
    public void PlayGate()
    {
        PlaySfx(gateClip);
    }
    public void PlayRoomTrans()
    {
        PlaySfx(roomTransClip);
    }
    public void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        int index = Random.Range(0, footstepSounds.Length);

        PlaySfx(footstepSounds[index]);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMenuBGM()
    {
        if (bgmSource.clip == menuBGM)
            return;

        bgmSource.clip = menuBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        if (bgmSource.clip == clip)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}