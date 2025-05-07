using UnityEngine;

public class AsteroidMd : MonoBehaviour
{
    public GameObject asteroidSmPrefab; // Prefab for the asteroid to be instantiated
    public GameObject explosionPrefab; // Prefab for the explosion effect

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // Destroy the bullet
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instantiate the explosion effect at the asteroid's position
            for (int i = 0; i < 3; i++)
            {
                Instantiate(asteroidSmPrefab, transform.position, Quaternion.identity); // Instantiate two smaller asteroids at the asteroid's position
            }
            Destroy(gameObject); // Destroy the asteroid
        }
    }
}
