using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float diagonalModifier;
    
    private float horizontal;
    private float vertical;

    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    
    private float currentDashForce = 1f;
    private float dashCooldownCounter;
    
    private Rigidbody2D rb;
    private Animator anim;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.isPaused)
        {
            horizontal = 0f;
            vertical = 0f;
            anim.SetBool(IsMoving, false);
            rb.velocity = Vector2.zero;
            return;
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon)
        {
            anim.SetFloat(Horizontal, horizontal);
            anim.SetFloat(Vertical, vertical);
            anim.SetBool(IsMoving, true);
        }
        else
        {
            anim.SetBool(IsMoving, false);
        }

        if (dashCooldownCounter <= 0)
        {
            if (PlayerManager.stats.abilities.dash && Input.GetKeyDown(KeyCode.Space) &&
                (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon))
            {
                dashCooldownCounter = dashCooldown;
                currentDashForce = dashForce;
                StartCoroutine("DisableDash");
            }
        }
        else
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private IEnumerator DisableDash()
    {
        yield return new WaitForSeconds(dashDuration);
        currentDashForce = 1f;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPaused) return;
        
        if (Mathf.Abs(horizontal) > Mathf.Epsilon && Mathf.Abs(vertical) > Mathf.Epsilon)
        {
            horizontal *= diagonalModifier;
            vertical *= diagonalModifier;
        } 
        
        rb.velocity = new Vector2(horizontal, vertical) * movementSpeed * currentDashForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if (pickup != null)
        {
            pickup.Consume();
        }
    }
}
