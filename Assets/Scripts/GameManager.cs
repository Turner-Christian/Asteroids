using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static int SCORE;
    public TextMeshProUGUI scoreText; // Reference to the UI text element for displaying the score
    public GameObject asteroidBgPrefab; // Prefab for the background asteroid
    public GameObject asteroidMdPrefab; // Prefab for the medium asteroid
    public GameObject asteroidSmPrefab; // Prefab for the small asteroid
    public GameObject ufoPrefab; // Prefab for the UFO
    public GameObject shipPrefab; // Prefab for the ship
    public GameObject shipExplosionPrefab; // Prefab for
    public GameObject gameOverScreen; // Reference to the game over screen UI element
    public GameObject pauseScreen; // Reference to the game over screen UI element
    public float minSpawnDistance = 4f; // Safe radius around player
    public float maxSpawnDistance = 10f; // Camera boundary-ish
    public float minUFOSpawnDistance = 11f; // Minimum distance for UFO spawn outside the camera
    public float maxUFOSpawnDistance = 15f; // Maximum distance for UFO spawn outside the camera
    public Transform playerTransform; // Reference to the player's transform
    public UnityEngine.UI.Image[] livesIcons; // Array of UI images for lives
    public int livesRemaining;
    public bool isHighScore; // Flag to check if the current score is a high score
    private int numOfAsteroids; // Number of asteroids to spawn
    private Rigidbody2D shipRb; // Reference to the ship's Rigidbody2D component
    private List<GameObject> lifeIcons = new(); // List to hold the life icons

    private void Awake()
    {
        numOfAsteroids = 10; // Initial number of asteroids to spawn
        shipRb = shipPrefab.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship prefab
    }

    private void Start()
    {
        StartGame(); // Start the game
        InvokeRepeating(nameof(SpawnUFO), 10f, 15f); // Spawn UFOs every 10 seconds after an initial delay of 5 seconds
        isHighScore = false; // Initialize the high score flag to false
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if the Escape key is pressed
        {
            PauseGame(); // Call the PauseGame method to toggle pause state
        }
        scoreText.text = SCORE.ToString();
        NoAsteroids(); // Check if there are any asteroids left in the scene
    }

    private void Spawner()
    {

        if (playerTransform == null)
        {
            RespawnShip(); // Respawn the ship if it doesn't exist
        }
        for (int i = 0; i < numOfAsteroids; i++)
        {
            Vector2 spawnOffset = Vector2.zero;

            // Pick a point in a ring (between min and max distance)
            bool validPosition = false;
            while (!validPosition)
            {
                spawnOffset =
                    Random.insideUnitCircle.normalized
                    * Random.Range(minSpawnDistance, maxSpawnDistance);
                if (spawnOffset.magnitude >= minSpawnDistance)
                {
                    validPosition = true;
                }
            }

            Vector3 spawnPos =
                playerTransform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);

            GameObject selectedPrefab = ChooseAsteroid(); // Default prefab
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        }
    }

    private GameObject ChooseAsteroid()
    {
        // Randomly choose between the two prefabs
        if (Random.value < 0.5f)
        {
            return asteroidBgPrefab;
        }
        else
        {
            return asteroidMdPrefab;
        }
    }

    private void SpawnUFO()
    {
        Vector2 ufoSpawnPos = Random.insideUnitCircle.normalized * Random.Range(minUFOSpawnDistance, maxUFOSpawnDistance);
        GameObject newUfo = Instantiate(ufoPrefab, ufoSpawnPos, Quaternion.identity); // Spawn the UFO at the calculated position
        Ufo ufoScript = newUfo.GetComponent<Ufo>();
        ufoScript.gameManager = this;
    }

    private void RespawnShip()
    {
        if (livesRemaining > 0)
        {
            GameObject newShip = Instantiate(shipPrefab, new Vector2(0, 0), Quaternion.identity);
            playerTransform = newShip.transform; // Update the playerTransform reference to the new ship

            // Also assign GameManager to the ship script so it can call back
            Ship shipScript = newShip.GetComponent<Ship>();
            shipScript.gameManager = this;
            shipScript.isDead = false; // Reset the dead state of the ship
        }
    }

    public void Death()
    {
        if (playerTransform == null) return; // Prevents errors from destroyed ship
        Vector3 deathPos = playerTransform.position; // Get the position of the ship at the time of death
        Instantiate(shipExplosionPrefab, deathPos, Quaternion.identity); // Instantiate the explosion effect at the ship's position
        Destroy(playerTransform.gameObject);
        LoseLife(); // Decrease the life count

        if (livesRemaining > 0)
        {
            Invoke(nameof(RespawnShip), 2f); // Wait 2 seconds before respawning
        }
    }

    [System.Obsolete]
    public void LoseLife()
    {
        livesRemaining--;
        livesIcons[livesRemaining].gameObject.SetActive(false); // Deactivate the life icon
        if (livesRemaining <= 0)
        {
            gameOverScreen.SetActive(true); // Show the game over screen

            // Check if the current score is a high score
            if (SCORE > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", SCORE); // Update the high score if the current score is higher
                PlayerPrefs.Save(); // Save the PlayerPrefs
                isHighScore = true; // Set the high score flag to true
                Debug.Log("New High Score: " + SCORE);
            }
            else
            {
                isHighScore = false; // Reset the high score flag if no new high score
            }

            // Notify BlinkingText script to update the text visibility
            var blinkingText = FindObjectOfType<BlinkingText>(); // Find the BlinkingText in the scene
            if (blinkingText != null)
            {
                blinkingText.StartBlinking(); // Restart the blinking if needed
            }
        }
    }

    public void DestroyAllAsteroids()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid"); // Find all asteroids in the scene
        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid); // Destroy each asteroid
        }
    }

    public void NoAsteroids()
    {
        // Check if there are any asteroids left in the scene
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            StartLevel(); // Start a new level
        }
    }

    public void StartGame()
    {
        DestroyAllAsteroids(); // Destroy all existing asteroids
        SCORE = 0; // Reset the score to 0
        isHighScore = false; // Initialize the high score flag to false
        livesRemaining = 3; // Set the initial number of lives
        gameOverScreen.SetActive(false); // Hide the game over screen
        for (int i = 0; i < livesIcons.Length; i++)
        {
            livesIcons[i].gameObject.SetActive(true);
        }
        if (playerTransform != null)
        {
            Destroy(playerTransform.gameObject); // Destroy the existing ship if it exists
            playerTransform = null; // Reset the playerTransform reference
        }
        RespawnShip(); // Respawn the ship
        Spawner(); // Spawn the initial asteroids
    }

    public void StartLevel()
    {
        SCORE += 500; // Increase the score by 100 for completing a level
        numOfAsteroids += 2; // Increase the number of asteroids for the next level
        Spawner();
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1; // Resume the game by setting time scale to 1
            pauseScreen.SetActive(false); // Hide the pause screen
        }
        else
        {
            Time.timeScale = 0; // Pause the game by setting time scale to 0
            pauseScreen.SetActive(true); // Show the pause screen
        }
    }

}
