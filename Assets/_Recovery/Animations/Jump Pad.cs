using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Settings")]
    public float launchForce = 15f; // Adjust this in Inspector for higher jumps
    public string playerTag = "Player";

    [Header("Animation")]
    public Animator anim;
    public string bounceTrigger = "Bounce";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object stepping on the pad is the player
        if (other.CompareTag(playerTag))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Reset Y velocity so the launch is consistent even if falling
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

                // Apply the launch force
                rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);

                // Trigger animation if you have one
                if (anim != null)
                {
                    anim.SetTrigger(bounceTrigger);
                }
            }
        }
    }
}