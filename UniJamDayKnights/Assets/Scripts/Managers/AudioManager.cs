using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Basic SFX")]
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip interactionClip;
    [SerializeField] private AudioClip leverClip;
    [SerializeField] private AudioClip spikeDeathClip;
    [SerializeField] private AudioClip fallDeathClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    public void PlaySpikeDeath()
    {
        PlaySfx(spikeDeathClip);
    }

    public void PlayFallDeath()
    {
        PlaySfx(fallDeathClip);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}