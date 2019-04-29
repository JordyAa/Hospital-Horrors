using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosionEffect;

    public Color trailColor;
    public GameObject[] trailObjects;
    public float timeBetweenTrails;
    private float timeBetweenTrailCounter;
    
    private SpellSettings spell;
    private string source;

    private CameraFollow cam;
    
    public void Initialise(SpellSettings spell, Vector2 direction, string source)
    {
        this.spell = spell;
        this.source = source;
        timeBetweenTrailCounter = timeBetweenTrails;

        cam = Camera.main.GetComponent<CameraFollow>();
        
        Destroy(gameObject, spell.lifeTime);
        GetComponent<Rigidbody2D>().velocity = direction * spell.projectileForce;
    }
    
    public void Initialise(SpellSettings spell, Vector2 direction, string source, Stats stats)
    {
        Initialise(spell, direction, source);
        ApplyStats(stats);
    }

    private void ApplyStats(Stats stats)
    {
        spell.minDamage = Mathf.FloorToInt(spell.minDamage * stats.weapon.minDamageModifier);
        spell.maxDamage = Mathf.FloorToInt(spell.maxDamage * stats.weapon.maxDamageModifier);
        
        spell.manaCost = Mathf.FloorToInt(spell.manaCost * stats.weapon.manaCostModifier);
        spell.cooldown *= stats.weapon.cooldownModifier;
        
        spell.projectileForce *= stats.weapon.projectileForceModifier;
    }

    private void Update()
    {
        if (timeBetweenTrailCounter <= 0)
        {
            timeBetweenTrailCounter = timeBetweenTrails;

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
            PlayerManager.instance.Hit(Random.Range(spell.minDamage, spell.maxDamage));
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeHit(Random.Range(spell.minDamage, spell.maxDamage));
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
