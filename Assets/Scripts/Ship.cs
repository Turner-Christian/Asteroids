using UnityEngine;
using System.Collections; // For using coroutines

public class Ship : MonoBehaviour
{
    public AudioSource fireSource;  // Reference to the AudioSource for firing sound
    public AudioClip fireSoundClip; // Reference to the AudioClip for firing sound
    public AudioSource thrustSource; // Reference to the AudioSource component
    public GameManager gameManager; // Reference to the GameManager object
    public GameObject bulletPrefab; // Prefab for the bullet to be instantiated
    public GameObject shipExplosionPrefab; // Prefab for the ship explosion effect
    public float thrustDamp = 0.2f; // Damping factor for rotation speed
    public float turnSpeed = 5f; // Speed at which the ship turns
    public float thrustForce = 10f; // Thrust force applied to the ship when moving forward
    public float bulletSpeed = 15f; // Speed of the bullet
    public bool isInvincible; // Flag to check if the ship is invincible
    public bool isDead = false; // Flag to check if the ship is dead
    private bool isThrusting; // Flag to check if the ship is thrusting
    public Transform firePoint; // Point from which the bullet
    private Rigidbody2D rb; // Reference to the ship's Rigidbody2D component
    private Animator animator; // Reference to the ship's Animator component
    private Vector2 shipVelocity;
    public float minPitch = 0.8f; // Minimum pitch value
    public float maxPitch = 1.2f; // Maximum pitch value

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship
        rb.linearDamping = thrustDamp; // Set linear damping to reduce linear speed over time
        animator = GetComponent<Animator>(); // Get the Animator component attached to the ship

        if (thrustSource == null)
        thrustSource = GetComponent<AudioSource>(); // Automatically grab the attached AudioSource
    }

    private void Start()
    {
        isInvincible = true; // Set the ship to be invincible at the start
        Invoke("DisableInvincibility", 2f); // Call the DisableInvincibility method after 2 seconds
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!isThrusting)
            {
                isThrusting = true; // Set the thrusting flag to true
                thrustSource.volume = 0.5f; // Start the sound with low volume, gradually increase.
                thrustSource.Play(); // Play the thrust sound when the ship is moving forward
            }
        }
        else
        {
            if (isThrusting)
            {
                isThrusting = false; // Set the thrusting flag to false
                StartCoroutine(FadeOutSound());
            }
        }

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

        if (isThrusting)
        {
            animator.SetBool("isMoving", true);
            rb.AddForce(transform.up * thrustForce);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // If the ship is already dead, do nothing


        if (collision.CompareTag("ufoBullet") && isInvincible)
        {
            gameManager.Death(); // Call the Death method in the GameManager when the ship collides with a UFO bullet
            return; // Exit the method to prevent further processing
        }

        if (!isInvincible)
        {
            isDead = true; // Set the ship to be dead
            gameManager.Death(); // Call the Death method in the GameManager when the ship collides with an object
        }
    }

    private IEnumerator FadeOutSound()
    {
        while (thrustSource.volume > 0)
        {
            thrustSource.volume -= Time.deltaTime * 2; // Fade out smoothly
            yield return null;
        }
        thrustSource.Stop();
        thrustSource.volume = 1; // Reset volume to normal
    }

    private void DisableInvincibility()
    {
        animator.SetBool("isMortal", true); // Set the animator parameter to indicate the ship is mortal
        isInvincible = false; // Set the ship to be not invincible
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

        if (fireSource != null && fireSoundClip != null)
        {
            fireSource.pitch = Random.Range(minPitch, maxPitch);
            fireSource.PlayOneShot(fireSoundClip); // Play the firing sound
        }
    }
}
