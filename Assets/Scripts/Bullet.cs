using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // Time before the bullet is destroyed

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
