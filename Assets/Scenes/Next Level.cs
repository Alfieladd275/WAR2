using UnityEngine;
using UnityEngine.SceneManagement; // Essential for switching scenes

public class SceneTeleporter : MonoBehaviour
{
    [Header("Transition Settings")]
    public string nextSceneName; // Type the exact name of your next scene here

    [Header("Audio")]
    public AudioClip teleportSound;
    [Range(0f, 1f)]
    public float volume = 0.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing that walked into the trigger is the Player
        if (other.CompareTag("Player"))
        {
            // 1. Play the sound on the player so it doesn't get cut off
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (teleportSound != null && playerAudio != null)
            {
                playerAudio.PlayOneShot(teleportSound, volume);
            }

            // 2. Load the next scene
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("You forgot to type the Scene Name in the Inspector!");
            }
        }
    }
}