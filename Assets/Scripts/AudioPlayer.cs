using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    [SerializeField] AudioSource sounds;
    [SerializeField] float pitchModifier;

    public void PlaySound(AudioClip sound)
    {
        sounds.pitch = Random.Range(1.0f - pitchModifier, 1.0f + pitchModifier);
        sounds.PlayOneShot(sound);

    }
}
