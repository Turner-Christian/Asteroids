using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // Time before the bullet is destroyed

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Destroy(gameObject); // Destroy the bullet
        }
    }
}
