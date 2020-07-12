using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioSource audioSource = null;
    public List<AudioClip> soundClips = new List<AudioClip>();

    void OnEnable()
    {
        audioSource.PlayOneShot(soundClips[Random.Range(0, soundClips.Count)]);
    }
}