using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightPulse : MonoBehaviour
{
    [SerializeField] private float minIntensity = 1f;
    [SerializeField] private float maxIntensity = 2f;
    [SerializeField] private float pulseSpeed = 2f;

    private Light2D light2D;
    private float timeOffset;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float wave =
            (Mathf.Sin(Time.time * pulseSpeed + timeOffset) + 1f) * 0.5f;

        light2D.intensity =
            Mathf.Lerp(minIntensity, maxIntensity, wave);
    }
}