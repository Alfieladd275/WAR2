using UnityEngine;

public class MPPickup : MonoBehaviour
{
    public float mpAmount = 15f;

    [Header("Audio")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] // Adds the slider in the Inspector
    public float volume = 1f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Collider2D playerCollider = playerObj.GetComponent<Collider2D>();
            Collider2D[] myColliders = GetComponents<Collider2D>();

            if (playerCollider != null)
            {
                foreach (Collider2D col in myColliders)
                {
                    if (!col.isTrigger)
                    {
                        Physics2D.IgnoreCollision(col, playerCollider);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Combat player = other.GetComponent<Combat>();

            if (player != null && player.currentMP < player.maxMP)
            {
                // Play through the Player's AudioSource so it's loud and clear
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (pickupSound != null && playerAudio != null)
                {
                    playerAudio.PlayOneShot(pickupSound, volume);
                }

                player.RestoreMP(mpAmount); // Uses the float fix
                Destroy(gameObject);
            }
        }
    }
}