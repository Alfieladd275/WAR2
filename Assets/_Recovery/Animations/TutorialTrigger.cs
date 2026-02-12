using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string messageContent;
    public bool onlyTriggerOnce = true;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure your player object has the "Player" Tag!
        if (other.CompareTag("Player") && !activated)
        {
            // We changed 'TutorialManager' to 'TypeWriter' to match your script name
            if (TypeWriter.Instance != null)
            {
                TypeWriter.Instance.ShowDialogue(messageContent);
                if (onlyTriggerOnce) activated = true;
            }
            else
            {
                Debug.LogError("TutorialTrigger: Could not find TypeWriter.Instance! Is the script on your Canvas?");
            }
        }
    }
}