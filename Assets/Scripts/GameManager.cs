using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject asteroidBgPrefab; // Prefab for the background asteroid
    public GameObject asteroidSmPrefab; // Prefab for the small asteroid
    public GameObject asteroidMdPrefab; // Prefab for the medium asteroid
    public float spawnRadius = 12f; // Distance from center to spawn

    private void Spawner()
    {
        // TODO: Implement the spawning logic for asteroids
        // Example: Instantiate an asteroid at a random position within the spawn radius
    }
}
