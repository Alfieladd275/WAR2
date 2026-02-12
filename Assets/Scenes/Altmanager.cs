using UnityEngine;
using UnityEngine.SceneManagement;

public class AltManager : MonoBehaviour
{
    [Header("Targeting")]
    public Combat playerCombat;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject youWinPanel;

    private bool gameHasEnded = false;

    void Start()
    {
        Time.timeScale = 1f;
        // Clean start: hide everything
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (youWinPanel != null) youWinPanel.SetActive(false);
    }

    void Update()
    {
        // Only check for death if the game hasn't ended via winning or losing yet
        if (playerCombat != null && !gameHasEnded)
        {
            if (playerCombat.isDead && playerCombat.spriteRend.enabled == false)
            {
                ShowGameOver();
            }
        }
    }

    public void ShowGameOver()
    {
        if (gameHasEnded) return;
        gameHasEnded = true;

        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        // Ensure the Win panel is definitely off
        if (youWinPanel != null) youWinPanel.SetActive(false);

        SetupUIControls();
    }

    public void ShowYouWin()
    {
        if (gameHasEnded) return;
        gameHasEnded = true;

        Time.timeScale = 0f;
        if (youWinPanel != null) youWinPanel.SetActive(true);
        // Ensure the Lose panel is definitely off
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        SetupUIControls();
        Debug.Log("Win State Triggered Successfully!");
    }

    void SetupUIControls()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Direct Level Links
    public void LoadTutorial() { Time.timeScale = 1f; SceneManager.LoadScene("Tutorial"); }
    public void LoadLevel1() { Time.timeScale = 1f; SceneManager.LoadScene("Level1"); }
    public void LoadNightmare() { Time.timeScale = 1f; SceneManager.LoadScene("Nightmare Mode"); }
}