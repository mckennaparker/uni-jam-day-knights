using UnityEngine;
using UnityEngine.Audio;

public class BoxAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PushableBox pushableBox;

    private void Update()
    {
        float horizontalSpeed =
            Mathf.Abs(pushableBox.RelativeVelocity.x);

        if (horizontalSpeed > 0.1f)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
