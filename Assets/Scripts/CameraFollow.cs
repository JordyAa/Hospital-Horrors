using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    private Transform player;
    
    private Animator anim;
    private static readonly int DoShake = Animator.StringToHash("DoShake");

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, followSpeed);
        transform.position = new Vector3(newPos.x, newPos.y, -10f);
    }

    public void Shake()
    {
        anim.SetTrigger(DoShake);
    }
}
