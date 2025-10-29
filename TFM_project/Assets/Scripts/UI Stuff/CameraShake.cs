using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 3f;     //duration of each shake
    public float shakeMagnitude = 0.1f;  //how strong the shake is
    public float shakeInterval = 10f;    //time between shakes

    [Header("Audio Settings")]
    public AudioClip rumbleClip;

    private AudioSource audioSource;
    private Vector3 originalPos;
    private bool isShaking = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            audioSource.loop = true;
            audioSource.Play();
        }

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset camera and stop sound
        transform.localPosition = originalPos;
        audioSource.Stop();
        isShaking = false;
    }
}