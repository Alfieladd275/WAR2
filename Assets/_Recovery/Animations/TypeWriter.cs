using UnityEngine;
using System.Collections;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public static TypeWriter Instance;

    [Header("UI References")]
    public GameObject tutorialParent;
    public TextMeshProUGUI textMesh;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float displayTimeAfterDone = 3f;

    [Header("Audio")]
    public AudioClip typeSoundLoop;
    private AudioSource audioSource;
    private Coroutine typingRoutine;

    void Awake()
    {
        Instance = this;
        if (tutorialParent != null) tutorialParent.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void ShowDialogue(string message)
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        if (tutorialParent != null) tutorialParent.SetActive(true);
        typingRoutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        textMesh.text = "";
        if (typeSoundLoop != null) { audioSource.clip = typeSoundLoop; audioSource.Play(); }

        foreach (char letter in message.ToCharArray())
        {
            textMesh.text += letter;
            // Use Realtime so it types while the game is paused!
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        if (audioSource != null) audioSource.Stop();

        yield return new WaitForSecondsRealtime(displayTimeAfterDone);
        if (tutorialParent != null) tutorialParent.SetActive(false);
        typingRoutine = null;
    }
}