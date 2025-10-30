using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 3f;
    public float shakeMagnitude = 0.1f;
    public float shakeInterval = 10f;

    [Header("Audio Settings")]
    public AudioClip rumbleClip;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    public float maxVolume = 1f;

    private AudioSource audioSource;
    private Vector3 originalPos;
    private bool isShaking = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        originalPos = transform.localPosition;
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeInterval);
            yield return StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        isShaking = true;

        if (rumbleClip)
        {
            audioSource.clip = rumbleClip;
            audioSource.volume = 0f;
            audioSource.Play();

            // Fade in sound
            yield return StartCoroutine(FadeAudio(0f, maxVolume, fadeInTime));
        }

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade out sound
        if (rumbleClip)
        {
            yield return StartCoroutine(FadeAudio(audioSource.volume, 0f, fadeOutTime));
            audioSource.Stop();
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }

    IEnumerator FadeAudio(float startVolume, float endVolume, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = endVolume;
    }
}