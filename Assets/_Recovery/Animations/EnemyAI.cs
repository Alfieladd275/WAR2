using UnityEngine;

public class EnemyAI : Enemy
{
    [Header("Detection")]
    public float detectionRange = 8f;

    [Header("Melee Movement")]
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.2f;

    [Header("Melee Attack")]
    public int damage = 10;
    public float attackRate = 1.5f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;

    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private bool facingRight = false;

    protected override void Start()
    {
        base.Start(); // This ensures the base Enemy script sets up the AudioSource
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (distance > stoppingDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopAndAttack();
            }
            HandleFlip();
        }
        else
        {
            StayIdle();
        }
    }

    void StayIdle()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("isRunning", false);
    }

    public void Attack() // Triggered by Animation Event
    {
        // 1. Play the attack sound we set up in the Enemy script
        PlayAttackSound();

        // 2. Deal the damage
        Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (playerHit != null)
        {
            playerHit.GetComponent<Combat>()?.TakeDamage(damage);
        }
    }

    void MoveTowardsPlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("isRunning", true);
    }

    void StopAndAttack()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("isRunning", false);

        if (Time.time >= nextAttackTime)
        {
            if (anim != null) anim.SetTrigger("Attack");
            nextAttackTime = Time.time + attackRate;
        }
    }

    void HandleFlip()
    {
        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}