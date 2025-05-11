using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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
    public GameObject shipPrefab; // Prefab for the ship
    public GameObject shipExplosionPrefab; // Prefab for
    public GameObject gameOverScreen; // Reference to the game over screen UI element
    public int numOfAsteroids = 10; // Number of asteroids to spawn
    public float minSpawnDistance = 4f; // Safe radius around player
    public float maxSpawnDistance = 10f; // Camera boundary-ish
    public Transform playerTransform; // Reference to the player's transform
    public UnityEngine.UI.Image[] livesIcons; // Array of UI images for lives
    public int livesRemaining;
    private Rigidbody2D shipRb; // Reference to the ship's Rigidbody2D component
    private List<GameObject> lifeIcons = new(); // List to hold the life icons

    private void Awake()
    {
        shipRb = shipPrefab.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship prefab
    }

    private void Start()
    {
        StartGame(); // Start the game
    }

    public void Update()
    {
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

    public void LoseLife()
    {
        livesRemaining--;
        livesIcons[livesRemaining].gameObject.SetActive(false); // Deactivate the life icon
        if (livesRemaining <= 0)
        {
            gameOverScreen.SetActive(true); // Show the game over screen
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
        Spawner();
    }
}
