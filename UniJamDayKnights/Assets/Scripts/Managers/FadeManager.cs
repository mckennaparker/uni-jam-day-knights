using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image fadeImage;

    [Header("Normal Fade")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("Restart Fade")]
    [SerializeField] private float restartFadeOutDuration = 0.2f;
    [SerializeField] private float restartFadeInDuration = 0.25f;

    [Header("Final Transition")]
    [SerializeField] private float finalFadeDuration = 3f;
    [SerializeField] private float finalWhiteHoldDuration = 1f;

    private bool isFading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage == null)
        {
            Debug.LogError("FadeManager: Fade Image is not assigned.");
            return;
        }

        SetFadeImage(Color.black, 1f);
    }

    private void Start()
    {
        StartCoroutine(FadeInFromBlack());
    }

    public void FadeToScene(int sceneIndex)
    {
        if (isFading)
            return;

        StartCoroutine(NormalSceneTransition(sceneIndex));
    }
    public void FadeToScene(string sceneName)
    {
        if (isFading)
            return;

        StartCoroutine(NormalSceneTransition(sceneName));
    }

    public void FadeRestartScene(float delayBeforeFade = 0f)
    {
        if (isFading)
            return;

        StartCoroutine(RestartSceneTransition(delayBeforeFade));
    }
    public void FadeToFinalScene(int sceneIndex)
    {
        if (isFading)
            return;

        StartCoroutine(FinalSceneTransition(sceneIndex));
    }

    private IEnumerator NormalSceneTransition(int sceneIndex)
    {
        isFading = true;

        AudioManager.Instance?.PlayRoomTrans();

        yield return Fade(Color.black, 0f, 1f, fadeOutDuration);

        SceneManager.LoadScene(sceneIndex);

        yield return null;

        yield return Fade(Color.black, 1f, 0f, fadeInDuration);

        isFading = false;
    }

    private IEnumerator NormalSceneTransition(string sceneName)
    {
        isFading = true;

        AudioManager.Instance?.PlayRoomTrans();

        yield return Fade(Color.black, 0f, 1f, fadeOutDuration);

        SceneManager.LoadScene(sceneName);

        yield return null;

        yield return Fade(Color.black, 1f, 0f, fadeInDuration);

        isFading = false;
    }

    private IEnumerator RestartSceneTransition(float delayBeforeFade)
    {
        isFading = true;

        if (delayBeforeFade > 0f)
            yield return new WaitForSecondsRealtime(delayBeforeFade);

        yield return Fade(
            Color.black,
            0f,
            1f,
            restartFadeOutDuration
        );

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        yield return null;

        yield return Fade(
            Color.black,
            1f,
            0f,
            restartFadeInDuration
        );

        isFading = false;
    }
    private IEnumerator FinalSceneTransition(int sceneIndex)
    {
        isFading = true;

        AudioManager.Instance?.PlaylongJingle();

        yield return Fade(Color.white, 0f, 1f, finalFadeDuration);

        yield return new WaitForSeconds(finalWhiteHoldDuration);

        SceneManager.LoadScene(sceneIndex);

        yield return null;

        yield return Fade(Color.white, 1f, 0f, finalFadeDuration);

        isFading = false;
    }

    private IEnumerator FadeInFromBlack()
    {
        isFading = true;

        yield return Fade(Color.black, 1f, 0f, fadeOutDuration);

        isFading = false;
    }

    private IEnumerator Fade(
        Color color,
        float startAlpha,
        float endAlpha,
        float duration)
    {
        if (fadeImage == null)
            yield break;

        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;

        SetFadeImage(color, startAlpha);

        if (duration <= 0f)
        {
            SetFadeImage(color, endAlpha);
        }
        else
        {
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;

                float progress = Mathf.Clamp01(elapsedTime / duration);
                float alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                SetFadeImage(color, alpha);

                yield return null;
            }

            SetFadeImage(color, endAlpha);
        }

        if (endAlpha <= 0f)
            fadeImage.gameObject.SetActive(false);
    }

    private void SetFadeImage(Color color, float alpha)
    {
        color.a = alpha;
        fadeImage.color = color;
    }
}