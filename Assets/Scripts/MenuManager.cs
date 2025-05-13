using TMPro; // Assuming you are using TextMeshPro for UI text
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text highScoreText; // Reference to the UI text element for displaying the high score
    public TMP_Text helpText; // Reference to the UI text element for displaying the high score

    void Start()
    {
        // Load the high score from PlayerPrefs and display it in the UI
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void ShowHelp()
    {
        if (helpText.gameObject.activeSelf == true)
        {
            helpText.gameObject.SetActive(false); // Show the help text
        }
        else
        {
            helpText.gameObject.SetActive(true); // Hide the help text
        }
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Load the game scene
    }
}
