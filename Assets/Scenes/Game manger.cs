using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWatcher : MonoBehaviour
{
    [Header("Victory Settings")]
    public GameObject boss1;        // Drag the first Boss here
    public GameObject boss2;        // Drag the second Boss here
    public GameObject victoryPanel; // Drag the "Victory UI" parent here
    public string nightmareSceneName = "NightmareMode";

    private bool hasWon = false;

    void Update()
    {
        // 1. Check if BOTH bosses are destroyed and we haven't won yet
        if (boss1 == null && boss2 == null && !hasWon)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        hasWon = true;

        if (victoryPanel != null)
        {
            // Freeze the game
            Time.timeScale = 0f;

            // 2. Activate the main panel
            victoryPanel.SetActive(true);

            // 3. ACTIVATE ALL CHILDREN
            foreach (Transform child in victoryPanel.transform)
            {
                child.gameObject.SetActive(true);
            }

            // 4. Unlock mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LoadNightmareMode()
    {
        // Remember to unfreeze time before switching scenes!
        Time.timeScale = 1f;
        SceneManager.LoadScene(nightmareSceneName);
    }

    // --- ADDED OPTION ---
    public void LoadTutorial()
    {
        // Unfreeze time before switching to the Tutorial
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial");
    }
}