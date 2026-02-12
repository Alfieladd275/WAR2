using UnityEngine;

public class SimpleLoop : MonoBehaviour
{
    private AudioSource musicSource;
    public float skipEndSeconds = 0.5f; // Adjust this until the "cut" disappears!

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        // Make sure "Loop" is UNCHECKED in the Inspector
        musicSource.loop = false;
    }

    void Update()
    {
        // If the song is almost at the very end...
        if (musicSource.time >= musicSource.clip.length - skipEndSeconds)
        {
            // Jump back to the beginning immediately
            musicSource.time = 0;
            musicSource.Play();
        }
    }
}