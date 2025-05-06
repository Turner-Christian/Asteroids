using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] possibleSprites; // Array of possible asteroid sprites
    public float minTorque = -100f; // Minimum rotation force
    public float maxTorque = 100f; // Maximum rotation force
    public float minSpeed = .5f; // Minimum speed of the asteroid
    public float maxSpeed = 2f; // Maximum speed of the asteroid
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the asteroid
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the asteroid

        if(possibleSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, possibleSprites.Length); // Get a random index from the array of sprites
            spriteRenderer.sprite = possibleSprites[randomIndex]; // Set the sprite of the asteroid to a random one from the array
        }
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
    }

    void Start()
    {
        // Randomly set the rotation of the asteroid
        float randomTorque = Random.Range(minTorque, maxTorque);
        rb.AddTorque(randomTorque);

        // Randomly set the speed and direction of the asteroid
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        rb.AddForce(randomDirection * randomSpeed, ForceMode2D.Impulse);
    }
}
