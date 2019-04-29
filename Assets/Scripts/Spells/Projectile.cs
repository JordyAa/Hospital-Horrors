using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosionEffect;

    public Color trailColor;
    public GameObject[] trailObjects;
    public float timeBetweenTrails;
    private float timeBetweenTrailCounter;
    
    private SpellSettings spell;
    private Stats stats;
    private string source;

    private CameraFollow cam;
    
    public void Initialise(SpellSettings spell, Vector2 direction, string source)
    {
        this.spell = spell;
        this.source = source;
        timeBetweenTrailCounter = timeBetweenTrails * Random.Range(.9f, 1.1f);

        if (Camera.main != null)
        {
            cam = Camera.main.GetComponent<CameraFollow>();
        }

        Destroy(gameObject, spell.lifeTime);
        GetComponent<Rigidbody2D>().velocity =
            direction * spell.projectileForce * PlayerManager.stats.weapon.projectileForceModifier;
    }
    
    public void Initialise(SpellSettings spell, Vector2 direction, string source, Stats stats)
    {
        Initialise(spell, direction, source);
        this.stats = stats;
    }

    private void Update()
    {
        if (timeBetweenTrailCounter <= 0)
        {
            timeBetweenTrailCounter = timeBetweenTrails * Random.Range(.9f, 1f);

            int rnd = Random.Range(0, trailObjects.Length);
            GameObject trail = Instantiate(trailObjects[rnd], transform.position, Quaternion.identity);
            trail.GetComponent<SpriteRenderer>().color = trailColor;
        }
        else
        {
            timeBetweenTrailCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(source) || other.isTrigger ||
            source == "Player" && other.CompareTag("Shield")) return;
        
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.Hit(Mathf.FloorToInt(Random.Range(spell.minDamage, spell.maxDamage) *
                                                        Random.Range(.9f, 1.1f)));
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeHit(
                Mathf.FloorToInt(Random.Range(spell.minDamage, spell.maxDamage) * 
                                 stats.weapon.maxDamageModifier * Random.Range(.9f, 1.1f)));
        }
        else
        {
            AudioManager.instance.SpellHit();
        }

        if (source == "Player" && Random.Range(0f, 1f) <= PlayerManager.stats.elementalChance.explosive)
        {
            cam.Shake();
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            AudioManager.instance.SpellExplosion();
        }
        
        Destroy(gameObject);
    }
}
