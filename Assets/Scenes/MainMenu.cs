using UnityEngine;
using UnityEngine.SceneManagement; // Required to load levels

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartGame()
    {
        // Play click sound before moving to next scene
        if (clickSound != null)
        {
            // Note: Use PlayClipAtPoint if the Canvas is destroyed instantly
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }

        // Loads the next scene in the build index (usually your first level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!"); // Only shows in the editor
        Application.Quit(); // Closes the actual game exe
    }
}