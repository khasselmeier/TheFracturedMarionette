using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioClip[] footstepClips;       //assign a few footstep clips
    public float footstepVolume = 0.8f;
    public float footstepDuration = 1f;     //duration each step sound plays

    [Header("Random SFX Settings")]
    public List<AudioClip> randomSFXList;
    [Range(0f, 1f)] public float randomPlayChance = 0.3f;
    public float minRandomInterval = 5f;    // shortest wait before next SFX
    public float maxRandomInterval = 15f;   // longest wait before next SFX
    public float randomSFXVolume = 0.7f;
    public float maxRandomClipDuration = 15f; //max time a random clip will play

    [Header("Persistent SFX")]
    public AudioClip persistentClip;        //looping sounds
    public float persistentVolume = 0.5f;

    private AudioSource sfxSource;          //for footsteps and random SFX
    private AudioSource persistentSource;   //separate source for persistent loop

    private bool canPlayFootstep = true;

    private void Awake()
    {
        //main SFX source
        sfxSource = GetComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        //persistent looping source
        persistentSource = gameObject.AddComponent<AudioSource>();
        persistentSource.loop = true;
        persistentSource.playOnAwake = true;
        persistentSource.volume = persistentVolume;
        persistentSource.clip = persistentClip;
    }

    private void Start()
    {
        if (persistentClip != null)
            persistentSource.Play();

        //start multiple staggered random SFX coroutines
        if (randomSFXList.Count > 0)
        {
            foreach (AudioClip clip in randomSFXList)
                StartCoroutine(PlayRandomSFXIndependently(clip));
        }
    }

    //call from movement script when a footstep occurs
    public void PlayFootstep()
    {
        if (!canPlayFootstep || footstepClips.Length == 0)
            return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        sfxSource.PlayOneShot(clip, footstepVolume);
        StartCoroutine(FootstepCooldown());
    }

    private IEnumerator FootstepCooldown()
    {
        canPlayFootstep = false;
        yield return new WaitForSeconds(footstepDuration);
        canPlayFootstep = true;
    }

    private IEnumerator PlayRandomSFXIndependently(AudioClip clip)
    {
        while (true)
        {
            //wait a random time between min/max interval
            float waitTime = Random.Range(minRandomInterval, maxRandomInterval);
            yield return new WaitForSeconds(waitTime);

            //random chance to play this clip
            if (Random.value <= randomPlayChance)
            {
                //create a temporary AudioSource for this clip
                AudioSource tempSource = gameObject.AddComponent<AudioSource>();
                tempSource.clip = clip;
                tempSource.volume = randomSFXVolume;
                tempSource.Play();

                Debug.Log($"[SFXManager] Random SFX playing: {clip.name} (max {maxRandomClipDuration}s)");

                //wait for either the clip to finish or 15 seconds, whichever comes first
                yield return new WaitForSeconds(Mathf.Min(clip.length, maxRandomClipDuration));

                tempSource.Stop();
                Destroy(tempSource);
                Debug.Log($"[SFXManager] Random SFX stopped: {clip.name}");
            }
        }
    }
}