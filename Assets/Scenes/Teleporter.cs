using UnityEngine;

public class SameSceneTeleporter : MonoBehaviour
{
    [Header("Destination")]
    public Transform destination; // Drag the "Target Location" object here

    [Header("Audio")]
    public AudioClip teleportSound;
    [Range(0f, 1f)]
    public float volume = 0.7f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (destination != null)
            {
                // 1. Play the sound on the player
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (teleportSound != null && playerAudio != null)
                {
                    playerAudio.PlayOneShot(teleportSound, volume);
                }

                // 2. Move the player to the destination's position
                other.transform.position = destination.position;
            }
            else
            {
                Debug.LogWarning("Teleporter A is missing its Destination B!");
            }
        }
    }
}