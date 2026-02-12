using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over Settings")]
    public GameObject player;       // Drag your Player object here
    public GameObject gameOverPanel; // Drag your Game Over UI parent here

    private bool isGameOver = false;

    void Update()
    {
        // If the player is destroyed/null and game isn't over yet
        if (player == null && !isGameOver)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            // 1. Freeze the game
            Time.timeScale = 0f;

            // 2. Show the Game Over UI
            gameOverPanel.SetActive(true);

            // 3. Turn on all buttons/text inside (Restart, Quit, etc.)
            foreach (Transform child in gameOverPanel.transform)
            {
                child.gameObject.SetActive(true);
            }

            // 4. Unlock mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // --- BUTTON FUNCTIONS ---

    public void RestartLevel()
    {
        // Unfreeze time first!
        Time.timeScale = 1f;
        // Reload the scene we are currently in
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exiting...");
        Application.Quit(); // This works in the actual build
    }
}