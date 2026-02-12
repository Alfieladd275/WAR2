using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    [Header("UI References")]
    public Slider hpSlider;
    public Slider mpSlider;
    public GameObject gameOverUI; // Keep this for reference, though AltGameManager handles the panel

    [Header("Stats")]
    public int maxHP = 100;
    public int maxMP = 50;
    public int currentHP;
    public float currentMP;

    [Header("Combat")]
    public int lightAttackDamage = 10;
    public int specialAttackDamage = 25;
    public int specialAttackCost = 20;
    public float attackRange = 1.5f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Audio Cues")]
    public AudioClip swingSound;
    public AudioClip hitSound;
    public AudioClip specialAttackSound;
    private AudioSource audioSource;

    [Header("Special Attack Settings")]
    public float specialAttackDuration = 0.8f;

    [Header("References")]
    public Animator animator;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRend; // Set to public for manager access

    private bool isLocked = false;
    private bool facingRight = true;
    public bool isDead = false; // Set to public so AltGameManager can see status

    [Header("Regen")]
    public float mpRegenRate = 2f;

    void Start()
    {
        Time.timeScale = 1f;
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        currentMP = maxMP;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (animator != null) animator.SetBool("isDead", false);

        if (hpSlider != null) { hpSlider.maxValue = maxHP; hpSlider.value = currentHP; }
        if (mpSlider != null) { mpSlider.maxValue = maxMP; mpSlider.value = currentMP; }

        // We don't disable UI here anymore; we use your "Check Logic" in the inspector
    }

    void Update()
    {
        if (isDead) return;

        HandleFacing();

        bool isMoving = rb.linearVelocity.magnitude > 0.1f;

        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            animator.SetTrigger("LightAttack");
            PlaySound(swingSound);
        }

        if (Input.GetMouseButtonDown(1) && !isMoving && !isLocked && currentMP >= specialAttackCost)
        {
            StartCoroutine(SpecialAttackRoutine());
        }

        RegenerateMP();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;
        if (hpSlider != null) hpSlider.value = currentHP;
        StartCoroutine(HitFlash());
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // NEW: Unparent camera so it doesn't vanish
        if (Camera.main != null && Camera.main.transform.parent == transform)
        {
            Camera.main.transform.parent = null;
        }

        animator.SetBool("isDead", true);
        animator.Play("Death");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        // Changed to the routine that talks to the Manager
        StartCoroutine(CleanupAfterDeath());
    }

    IEnumerator CleanupAfterDeath()
    {
        // 1. Wait for animation (Real Time so it works if game freezes)
        yield return new WaitForSecondsRealtime(2.0f);

        // 2. Hide visuals WITHOUT destroying (so the script stays alive)
        if (spriteRend != null) spriteRend.enabled = false;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        // 3. NEW: Call the AltGameManager on the Canvas
        AltManager gm = Object.FindFirstObjectByType<AltManager>();
        if (gm != null)
        {
            gm.ShowGameOver();
        }
        else
        {
            Debug.LogError("AltGameManager not found on Canvas!");
        }

        // Removed Destroy(gameObject) to keep the script and camera active
    }

    // --- ALL CORE FUNCTIONS BELOW REMAIN UNCHANGED ---
    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void QuitGame() { Application.Quit(); }
    public void Heal(float amount) { currentHP += (int)amount; if (currentHP > maxHP) currentHP = maxHP; if (hpSlider != null) hpSlider.value = currentHP; }
    public void RestoreMP(float amount) { currentMP += amount; if (currentMP > maxMP) currentMP = maxMP; if (mpSlider != null) mpSlider.value = currentMP; }
    public void LightAttack() => DealDamage(lightAttackDamage);
    public void SpecialAttack() => DealDamage(specialAttackDamage);
    IEnumerator SpecialAttackRoutine() { isLocked = true; currentMP -= specialAttackCost; if (mpSlider != null) mpSlider.value = currentMP; PlaySound(specialAttackSound); animator.SetTrigger("SpecialAttack"); yield return new WaitForSeconds(specialAttackDuration); isLocked = false; }
    void DealDamage(int damage) { Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer); foreach (Collider2D enemy in hitEnemies) { var enemyComponent = enemy.GetComponent<Enemy>(); if (enemyComponent != null) { PlaySound(hitSound); enemyComponent.TakeDamage(damage); break; } } }
    void PlaySound(AudioClip clip) { if (clip != null && audioSource != null) { audioSource.pitch = Random.Range(0.9f, 1.1f); audioSource.PlayOneShot(clip); } }
    void HandleFacing() { if (isLocked) return; float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x; if ((mouseX > transform.position.x && !facingRight) || (mouseX < transform.position.x && facingRight)) Flip(); }
    void Flip() { facingRight = !facingRight; Vector3 scale = transform.localScale; scale.x *= -1; transform.localScale = scale; }
    void RegenerateMP() { if (currentMP < maxMP) { currentMP += mpRegenRate * Time.deltaTime; if (mpSlider != null) mpSlider.value = currentMP; } }
    IEnumerator HitFlash() { spriteRend.color = Color.red; yield return new WaitForSeconds(0.1f); spriteRend.color = Color.white; }
}