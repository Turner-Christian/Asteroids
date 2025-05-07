using UnityEngine;

public class AsteroidSm : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab for the explosion effect

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // Destroy the bullet
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instantiate the explosion effect at the asteroid's position
            Destroy(gameObject); // Destroy the asteroid
        }
    }
}
