using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    void Start()
    {
        // Ignore all other bullets so they don't hit each other
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        foreach (GameObject b in bullets)
        {
            if (b != null && b != gameObject)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), b.GetComponent<Collider2D>());
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ONLY stop for the Player
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Combat>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        // ONLY stop for the Ground
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        // If it hits an "Enemy", it just keeps flying!
    }
}