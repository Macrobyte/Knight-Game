using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound;
    [SerializeField] float pitchModifier;
    int coinValue = 1;

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(1.0f - pitchModifier, 1.0f + pitchModifier);
        audioSource.PlayOneShot(sound);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        PlaySound();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        GameManager.Instance.AddScore(coinValue);

        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject,.2f);
    }
}
