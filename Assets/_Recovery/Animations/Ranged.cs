using UnityEngine;
using System.Collections;

public class FloatingRanger : Enemy
{
    [Header("Detection")]
    public float detectionRange = 12f; // Distance to start chasing/shooting

    [Header("Ranger Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 6f; // The "Sweet Spot" for shooting
    private Transform player;

    [Header("Ranger Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float projectileSpeed = 12f;

    protected override void Start()
    {
        // Setup sprite renderer from base Enemy class
        spriteRend = GetComponent<SpriteRenderer>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Start shooting cycle
        InvokeRepeating(nameof(ForceShoot), fireRate, fireRate);
    }

    void Update()
    {
        // Don't do anything if player is missing or enemy is dead
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // ONLY ACT IF WITHIN DETECTION RANGE
        if (distance <= detectionRange)
        {
            HandleFacing();

            // Movement logic: Move closer if too far, but stop at stopDistance
            if (distance > stopDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    void HandleFacing()
    {
        if (player.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void ForceShoot()
    {
        try
        {
            if (player == null || isDead || projectilePrefab == null || firePoint == null) return;

            float distance = Vector2.Distance(transform.position, player.position);

            // ONLY SHOOT IF WITHIN DETECTION RANGE
            if (distance > detectionRange) return;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Ignore collisions with self
            Collider2D bulletCol = bullet.GetComponent<Collider2D>();
            Collider2D myCol = GetComponent<Collider2D>();
            if (bulletCol != null && myCol != null) Physics2D.IgnoreCollision(bulletCol, myCol);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDir = (player.position - firePoint.position).normalized;
                rb.linearVelocity = shootDir * projectileSpeed;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Shooting failed but script recovered: " + e.Message);
        }
    }

    // Helps visualize the ranges in the Editor
    private void OnDrawGizmosSelected()
    {
        // Detection Range (Yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Shooting/Stop Distance (Red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}