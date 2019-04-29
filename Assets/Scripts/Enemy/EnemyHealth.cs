using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float currHealth;
    public float maxHealth;
    
    public GameObject healthBar;
    public Slider healthSlider;
    
    public GameObject lootDrop;
    public GameObject hpDrop;
    public GameObject manaDrop;
    
    public GameObject damageEffect;
    public GameObject burnEffect;
    public GameObject freezeEffect;

    private bool isBurning;
    [HideInInspector] public bool isDead;

    private EnemyCombat com;
    private Animator anim;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Start()
    {
        com = GetComponent<EnemyCombat>();
        anim = GetComponent<Animator>();

        maxHealth = Random.Range(maxHealth * .9f, maxHealth * 1.1f);
        currHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currHealth;
    }

    private void Update()
    {
        if (isBurning && isDead == false)
        {
            currHealth -= Time.deltaTime * 5f;
            CheckDeath();
        }
    }

    public void Burn()
    {
        burnEffect.SetActive(true);
        isBurning = true;
    }

    public void Freeze()
    {
        freezeEffect.SetActive(true);
        GetComponent<EnemyMovement>().isFreezing = true;
    }
    
    public void TakeHit(float damage)
    {
        currHealth -= damage;
        com.target = GameObject.FindGameObjectWithTag("Player").transform;

        Instantiate(damageEffect, transform.position, Quaternion.identity);
        CheckDeath();

        if (Random.Range(0f, 1f) <= PlayerManager.stats.elementalChance.burn)
        {
            Burn();
        }
        if (Random.Range(0f, 1f) <= PlayerManager.stats.elementalChance.freeze)
        {
            Freeze();
        }
        
        healthBar.SetActive(true);
        healthSlider.value = currHealth;        
    }

    private void CheckDeath()
    {
        if (currHealth <= 0)
        {
            AudioManager.instance.ZombieDie();
            
            
            anim.SetTrigger(IsDead);
            isDead = true;
            
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<EnemyCombat>());
            Destroy(GetComponent<EnemyMovement>());
            healthBar.SetActive(false);
            
            Instantiate(lootDrop, transform.position, Quaternion.identity);
            if (Random.Range(0f, 1f) <= PlayerManager.stats.dropChance.health)
            {
                Instantiate(hpDrop, transform.position + Vector3.left, Quaternion.identity);
            }
            if (Random.Range(0f, 1f) <= PlayerManager.stats.dropChance.mana)
            {
                Instantiate(manaDrop, transform.position + Vector3.right, Quaternion.identity);
            }
        }
        else if (isBurning == false)
        {
            AudioManager.instance.ZombieHit();
        }
    }
}
