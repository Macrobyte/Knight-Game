using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound;
    [SerializeField] float pitchModifier;
    int healthAmmount = 10;


    public void PlaySound()
    {
        audioSource.pitch = Random.Range(1.0f - pitchModifier, 1.0f + pitchModifier);
        audioSource.PlayOneShot(sound);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            if (health.GetCurrentHealth() == health.GetMaxHealth())
            {
                return;
            }
            else
            {
                PlaySound();
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                health.AddHealth(healthAmmount);

                if (other.gameObject.CompareTag("Player"))
                    Destroy(gameObject,1.5f);
            }
            

            
        }

        
    }
}
