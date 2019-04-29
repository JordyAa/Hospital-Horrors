using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float distanceToMove;
    public float followDistance;
    public float timeToKeepMoving;
    public float timeBetweenMovements;
    private float movementCounter;

    [HideInInspector] public bool isFreezing;
    
    private float horizontal;
    private float vertical;
    private Coroutine moveRoutine;
    
    private Rigidbody2D rb;
    private Animator anim;
    private EnemyCombat com;
    private EnemyHealth hp;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        com = GetComponent<EnemyCombat>();
        hp = GetComponent<EnemyHealth>();
        
        distanceToMove = Random.Range(distanceToMove * .9f, distanceToMove * 1.1f);
        timeToKeepMoving = Random.Range(timeToKeepMoving, timeToKeepMoving * 1.2f);
        timeBetweenMovements = Random.Range(timeBetweenMovements * .8f, timeBetweenMovements);
        
        movementCounter = timeBetweenMovements;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused || hp.isDead) return;
        
        if (com.target != null && Vector2.Distance(transform.position, com.target.position) <= followDistance)
        {
            Vector2 direction = (com.target.position - transform.position).normalized;
            horizontal = direction.x * distanceToMove;
            vertical = direction.y * distanceToMove;
            
            moveRoutine = StartCoroutine("Move");
            
            anim.SetFloat(Horizontal, horizontal);
            anim.SetFloat(Vertical, vertical);
            anim.SetBool(IsMoving, true);
        }
        else if (movementCounter <= 0)
        {
            movementCounter = (timeToKeepMoving + timeBetweenMovements) * Random.Range(.9f, 1.1f);

            horizontal = Random.Range(-distanceToMove, distanceToMove);
            vertical = (distanceToMove - Mathf.Abs(horizontal)) * Random.Range(0, 2) * 2 - 1;

            moveRoutine = StartCoroutine("Move");
            
            anim.SetFloat(Horizontal, horizontal);
            anim.SetFloat(Vertical, vertical);
            anim.SetBool(IsMoving, true);
        }
        else
        {
            movementCounter -= Time.deltaTime;
        }
    }
    
    private IEnumerator Move()
    {
        if (GameManager.instance.isPaused || hp.isDead) yield break;
        
        rb.velocity = new Vector2(horizontal, vertical);
        if (isFreezing)
        {
            rb.velocity *= .5f;
        }
        
        yield return new WaitForSeconds(timeToKeepMoving);

        rb.velocity = Vector2.zero;
        anim.SetBool(IsMoving, false);
    }

    private void OnCollisionEnter2D()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            
            rb.velocity = Vector2.zero;
            anim.SetBool(IsMoving, false);
        }
        
    }
}
