using UnityEngine;
using System.Collections;

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
    [SerializeField] private AudioClip shortJingleClip;
    [SerializeField] private AudioClip longJingleClip;
    [SerializeField] private AudioClip[] footstepSounds;

    private Coroutine bgmVolumeCoroutine;
    private Coroutine longJingleCoroutine;
    private float defaultBGMVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource != null)
            defaultBGMVolume = bgmSource.volume;
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

    public void PlayshortJingle()
    {
        PlaySfx(shortJingleClip);
    }

    public void PlaylongJingle()
    {
        if (longJingleCoroutine != null)
            StopCoroutine(longJingleCoroutine);

        longJingleCoroutine = StartCoroutine(LongJingleWithBGMDuckRoutine());
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
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMenuBGM()
    {
        if (bgmSource == null || menuBGM == null)
            return;

        if (bgmSource.clip == menuBGM && bgmSource.isPlaying)
            return;

        bgmSource.clip = menuBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null)
            return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void FadeBGMVolume(float targetVolume, float duration)
    {
        if (bgmSource == null)
            return;

        if (bgmVolumeCoroutine != null)
            StopCoroutine(bgmVolumeCoroutine);

        bgmVolumeCoroutine = StartCoroutine(FadeBGMVolumeRoutine(targetVolume, duration));
    }

    public void FadeBGMByMultiplier(float multiplier, float duration)
    {
        if (bgmSource == null)
            return;

        float targetVolume = defaultBGMVolume * multiplier;
        FadeBGMVolume(targetVolume, duration);
    }

    public void RestoreBGMVolume(float duration)
    {
        FadeBGMVolume(defaultBGMVolume, duration);
    }

    private IEnumerator FadeBGMVolumeRoutine(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float elapsedTime = 0f;

        if (duration <= 0f)
        {
            bgmSource.volume = targetVolume;
            bgmVolumeCoroutine = null;
            yield break;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, progress);
            yield return null;
        }

        bgmSource.volume = targetVolume;
        bgmVolumeCoroutine = null;
    }

    private IEnumerator LongJingleWithBGMDuckRoutine()
    {
        if (longJingleClip == null)
        {
            longJingleCoroutine = null;
            yield break;
        }

        FadeBGMByMultiplier(0.2f, 1f);
        PlaySfx(longJingleClip);

        yield return new WaitForSecondsRealtime(longJingleClip.length + 0.2f);

        RestoreBGMVolume(1f);
        longJingleCoroutine = null;
    }
}