using UnityEngine;
using System.Collections;

public class LightFlickering : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float flickerInterval = 5f;
    public float flickerDuration = 0.5f;
    public float minFlickerDelay = 0.05f;
    public float maxFlickerDelay = 0.15f;

    private Light spotLight;
    private bool isFlickering = false;

    void Start()
    {
        spotLight = GetComponent<Light>();

        if (spotLight == null)
        {
            enabled = false;
            return;
        }

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(flickerInterval);

            if (!isFlickering)
                StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker()
    {
        isFlickering = true;

        float endTime = Time.time + flickerDuration;
        while (Time.time < endTime)
        {
            spotLight.enabled = !spotLight.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));
        }

        //light ends up on
        spotLight.enabled = true;
        isFlickering = false;
    }
}