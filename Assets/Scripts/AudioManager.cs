using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set;  }

    public AudioSource soundEffectSource;
    public AudioClip[] playerGrunts;
    public AudioClip[] zombieGrunts;
    public AudioClip zombieGruntDie;

    public AudioClip[] spellHits;
    public AudioClip[] spellExplosions;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerHit()
    {
        soundEffectSource.pitch = Random.Range(.95f, 1.05f);
        soundEffectSource.PlayOneShot(playerGrunts[Random.Range(0, playerGrunts.Length)]);
    }
    
    public void ZombieHit()
    {
        soundEffectSource.pitch = Random.Range(.85f, .95f);
        soundEffectSource.PlayOneShot(zombieGrunts[Random.Range(0, zombieGrunts.Length)]);
    }

    public void ZombieDie()
    {
        soundEffectSource.pitch = Random.Range(.85f, .95f);
        soundEffectSource.PlayOneShot(zombieGruntDie);
    }

    public void SpellHit()
    {
        soundEffectSource.pitch = Random.Range(.95f, 1.05f);
        soundEffectSource.PlayOneShot(spellHits[Random.Range(0, spellHits.Length)]);
    }

    public void SpellExplosion()
    {
        soundEffectSource.pitch = Random.Range(.95f, 1.05f);
        soundEffectSource.PlayOneShot(spellExplosions[Random.Range(0, spellExplosions.Length)]);
    }
}
