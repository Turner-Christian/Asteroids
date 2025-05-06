using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab for the bullet to be instantiated
    public GameObject shipExplosionPrefab; // Prefab for the ship explosion effect
    public float thrustDamp = 0.2f; // Damping factor for rotation speed
    public float turnSpeed = 5f; // Speed at which the ship turns
    public float thrustForce = 10f; // Thrust force applied to the ship when moving forward
    public float bulletSpeed = 15f; // Speed of the bullet
    public Transform firePoint; // Point from which the bullet
    private Rigidbody2D rb; // Reference to the ship's Rigidbody2D component
    private Animator animator; // Reference to the ship's Animator component
    private Vector2 shipVelocity;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship
        rb.linearDamping = thrustDamp; // Set linear damping to reduce linear speed over time
        animator = GetComponent<Animator>(); // Get the Animator component attached to the ship
    }

    private void Update()
    {
        if (transform.position.y > 6f)
        {
            transform.position = new Vector3(transform.position.x, -6f, transform.position.z); // Wrap around the screen vertically
        }
        else if (transform.position.y < -6f)
        {
            transform.position = new Vector3(transform.position.x, 6f, transform.position.z); // Wrap around the screen vertically
        }
        if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, transform.position.z); // Wrap around the screen horizontally
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, transform.position.z); // Wrap around the screen horizontally
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
        {
            Shoot(); // Call the Shoot method to fire a bullet
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        if (horizontalInput != 0)
        {
            RotateShip(horizontalInput);
        }

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isMoving", true); // Set the animator parameter to indicate the ship is moving
            rb.AddForce(transform.up * thrustForce); // Apply thrust force in the direction the ship is facing
        }
        else
        {
            animator.SetBool("isMoving", false); // Set the animator parameter to indicate the ship is not moving
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Death(); // Call the Death method when the ship collides with an object
    }

    private void RotateShip(float Direction)
    {
        transform.Rotate(Vector3.forward * -Direction * turnSpeed * Time.deltaTime); // Rotate the ship based on input
    }

    private void Shoot()
    {
        shipVelocity = rb.linearVelocity; // Get the current velocity of the ship
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Combine ship's velocity with bullet's forward thrust
        bulletRb.linearVelocity = shipVelocity + (Vector2)(firePoint.up * bulletSpeed);
    }

    private void Death()
    {
        Instantiate(shipExplosionPrefab, transform.position, Quaternion.identity); // Instantiate the ship explosion effect at the ship's position

        ParticleSystem ps = shipExplosionPrefab.GetComponent<ParticleSystem>();
        shipVelocity = rb.linearVelocity; // Get the current velocity of the ship
        var velocityOverLifetime = ps.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.World;

        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(shipVelocity.x);
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(shipVelocity.y);

        Destroy(gameObject); // Destroy the ship
    }
}
