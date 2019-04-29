using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float projectileSpawnOffset;
    public SpellSettings equippedSpell;
    private float cooldownTimer;

    [HideInInspector] public Transform target;

    private Animator anim;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    protected void Start()
    {
        anim = GetComponent<Animator>();
        
        cooldownTimer = equippedSpell.cooldown + Random.Range(-.1f, .1f);
    }
    
    private void Update()
    {
        if (GameManager.instance.isPaused) return;
        
        if (cooldownTimer <= 0)
        {
            if (target != null)
            {
                cooldownTimer = equippedSpell.cooldown;
                
                Shoot();
                anim.SetTrigger(IsAttacking);
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        Vector2 position = transform.position;
        Vector2 targetPosition = target.position;
        Vector2 direction = (targetPosition - position).normalized;
            
        GameObject spell = Instantiate(
            equippedSpell.projectile,
            position + projectileSpawnOffset * direction,
            Quaternion.identity);

        spell.tag = tag;
        spell.GetComponent<Projectile>().Initialise(equippedSpell, direction, spell.tag);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
}
