using UnityEngine;
using System.Collections;
using System; // YOU NEED THIS FOR ACTION

public class Enemy : MonoBehaviour
{
    // This is the line your Game Manager is looking for!
    public static event Action OnBossDeath;

    [Header("Base Stats")]
    public int maxHealth = 50;
    protected int currentHealth;
    protected bool isDead = false;

    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float volume = 0.7f;
    protected AudioSource audioSource;

    protected SpriteRenderer spriteRend;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(attackSound, volume);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        StopAllCoroutines();
        StartCoroutine(HitFlash());
        if (currentHealth <= 0) Die();
    }

    protected IEnumerator HitFlash()
    {
        if (spriteRend != null)
        {
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRend.color = Color.white;
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position, volume);

        // If the enemy you killed is tagged "Boss", tell the Game Manager
        if (gameObject.CompareTag("Boss"))
        {
            OnBossDeath?.Invoke();
        }

        Destroy(gameObject);
    }
}