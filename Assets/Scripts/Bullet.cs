using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Speed of the bullet
    public float lifetime = 2f; // Time before the bullet is destroyed

    private Rigidbody2D rb; // Reference to the bullet's Rigidbody2D component

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the bullet
        rb.linearVelocity = transform.up * speed; // Set the bullet's velocity in the direction it is facing
        Destroy(gameObject, lifetime); // Destroy the bullet after a certain time
    }

}
