using UnityEngine;

public class Ufo : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager object
    public GameObject ufoExplosionPrefab; // Prefab for the UFO explosion effect
    public float speed = 0.1f; // Speed of the UFO
    private Vector2 dir;
    private Vector2 playerPosition; // Target position for the UFO to move towards
    private Vector2 targetPositionOffScreen; // Target position for the UFO to move towards
    public GameObject ufoBulletPrefab; // Prefab for the UFO bullet to be instantiated
    public Rigidbody2D bulletRb; // Reference to the bullet's Rigidbody2D component

    private enum UfoState
    {
        ApproachingPlayer,
        RandomMove,
        Exit,
    } // Enum to define the states of the UFO

    private UfoState currentState = UfoState.ApproachingPlayer; // Current state of the UFO
    private float randomMoveTimer = 0f; // Timer for random movement
    private float fireCooldown = 1.5f;
    private float fireTimer = 0f;

    public void Awake()
    {
        targetPositionOffScreen = Random.insideUnitCircle.normalized * Random.Range(10f, 12f); // Set the target position off-screen
    }

    private void Start()
    {
        if (gameManager.playerTransform == null)
            return; // Check if the player transform is valid
        playerPosition = gameManager.playerTransform.position; // Set the target position to the player's position
    }

    public void Update()
    {
        switch (currentState)
        {
            case UfoState.ApproachingPlayer:
                MoveToTarget(playerPosition);
                float distanceToPlayer = Vector2.Distance(transform.position, playerPosition); // Calculate the distance to the player
                if (distanceToPlayer < 3f)
                {
                    dir = Random.insideUnitCircle.normalized; // Set a random direction
                    randomMoveTimer = 1.5f;
                    currentState = UfoState.RandomMove; // Change state to random movement
                }
                break;

            case UfoState.RandomMove:
                transform.position += (Vector3)(dir * speed * Time.deltaTime); // Move in the random direction
                randomMoveTimer -= Time.deltaTime; // Decrease the timer
                if (randomMoveTimer <= 0f)
                {
                    currentState = UfoState.Exit; // Change state back to approaching player
                }
                break;
            case UfoState.Exit:
                MoveToTarget(targetPositionOffScreen);
                if (Vector2.Distance(transform.position, targetPositionOffScreen) < 0.5f)
                {
                    Destroy(gameObject);
                }
                break;
        }

        fireTimer -= Time.deltaTime; // Decrease the fire timer

        if (fireTimer <= 0f && IsVisibleFromCamera())
        {
            FireBullet(); // Call the method to fire a bullet
            fireTimer = fireCooldown; // Reset the fire timer
        }
    }

    private bool IsVisibleFromCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position); // Get the screen point of the UFO
        return viewportPos.z > 0
            && viewportPos.x > 0
            && viewportPos.x < 1
            && viewportPos.y > 0
            && viewportPos.y < 1; // Check if the UFO is within the camera's view
    }

    public void MoveToTarget(Vector2 targetPos)
    {
        if (targetPos == null)
            return; // Check if the target position is valid
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void FireBullet()
    {
        if (gameManager.playerTransform == null)
            return; // Check if the player transform is valid
        playerPosition = gameManager.playerTransform.position; // Set the target position to the player's position
        GameObject bullet = Instantiate(ufoBulletPrefab, transform.position, Quaternion.identity); // Instantiate the bullet prefab
        bulletRb = bullet.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the bullet
        bulletRb.linearVelocity = (playerPosition - (Vector2)transform.position).normalized * 5f; // Set the bullet's velocity towards the player
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.SCORE += 100; // Increment the score by 30 when the UFO is destroyed
            Vector2 deathPos = transform.position; // Get the position of the UFO at the time of death
            Instantiate(ufoExplosionPrefab, deathPos, Quaternion.identity); // Instantiate the explosion effect at the ship's position
            Destroy(other.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy the UFO
        }
    }
}
