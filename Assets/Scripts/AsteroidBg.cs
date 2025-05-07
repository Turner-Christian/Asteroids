using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AsteroidBg : MonoBehaviour
{
    public GameObject asteroidMdPrefab; // Prefab for the asteroid to be instantiated
    public GameObject explosionPrefab; // Prefab for the explosion effect

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.SCORE += 10; // Increment the score by 10 when an asteroid is destroyed
            Destroy(other.gameObject); // Destroy the bullet
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instantiate the explosion effect at the asteroid's position
            for (int i = 0; i < 2; i++)
            {
                Instantiate(asteroidMdPrefab, transform.position, Quaternion.identity); // Instantiate two smaller asteroids at the asteroid's position
            }
            Destroy(gameObject); // Destroy the asteroid
        }
    }
}
