using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab for the bullet to be instantiated 
    public float thrustDamp = 0.2f; // Damping factor for rotation speed
    public float turnSpeed = 5f; // Speed at which the ship turns
    public float thrustForce = 10f; // Thrust force applied to the ship when moving forward
    private Rigidbody2D rb; // Reference to the ship's Rigidbody2D component
    private Animator animator; // Reference to the ship's Animator component

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the ship
        rb.linearDamping = thrustDamp; // Set linear damping to reduce linear speed over time
        animator = GetComponent<Animator>(); // Get the Animator component attached to the ship
    }

    void Update()
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

    void FixedUpdate()
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

    void RotateShip(float Direction)
    {
        transform.Rotate(Vector3.forward * -Direction * turnSpeed * Time.deltaTime); // Rotate the ship based on input
    }

    void Shoot()
    {
        // TODO: Implement shooting logic
    }
}
