using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject asteroidBgPrefab; // Prefab for the background asteroid
    public GameObject asteroidMdPrefab; // Prefab for the medium asteroid
    public GameObject shipPrefab; // Prefab for the ship
    public GameObject shipExplosionPrefab; // Prefab for
    public int numOfAsteroids = 10; // Number of asteroids to spawn
    public float minSpawnDistance = 4f; // Safe radius around player
    public float maxSpawnDistance = 10f; // Camera boundary-ish
    public Transform playerTransform; // Reference to the player's transform
    private Rigidbody2D shipRb; // Reference to the ship's Rigidbody2D component
    private int score;
    private int lives;

    private void Awake()
    {
        shipRb = shipPrefab.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship prefab
        // Initialize the game state
        score = 0;
        lives = 3; // Set the number of lives for the player
    }

    private void Start()
    {
        Spawner();
    }

    private void Spawner()
    {
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
        if (lives > 0)
        {
            GameObject newShip = Instantiate(shipPrefab, new Vector2(0, 0), Quaternion.identity);
            playerTransform = newShip.transform; // Update the playerTransform reference to the new ship

            // Also assign GameManager to the ship script so it can call back
            Ship shipScript = newShip.GetComponent<Ship>();
            shipScript.gameManager = this;

            lives--;
        }
        else
        {
            // Game over logic here
            Debug.Log("Game Over!");
        }
    }

    public void Death()
    {
        Vector3 deathPos = playerTransform.position; // Get the position of the ship at the time of death
        Instantiate(shipExplosionPrefab, deathPos, Quaternion.identity); // Instantiate the explosion effect at the ship's position
        Destroy(playerTransform.gameObject);
        Invoke(nameof(RespawnShip), 2f); // Wait 2 seconds before respawning
    }
}
