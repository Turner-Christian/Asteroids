using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource explosionSource; // Reference to the AudioSource component
    public AudioClip explosionSound;   // Reference to the explosion sound clip
    public float minPitch = 0.8f;      // Minimum pitch value
    public float maxPitch = 1.2f;      // Maximum pitch value

    private void Awake()
    {
        // Ensure the AudioSource component is attached
        if (explosionSource == null)
        {
            explosionSource = GetComponent<AudioSource>(); // Grab the AudioSource attached to the prefab
        }

        if (explosionSound != null && explosionSource != null)
        {
            // Randomize the pitch before playing the sound
            explosionSource.pitch = Random.Range(minPitch, maxPitch);

            // Play the explosion sound
            explosionSource.PlayOneShot(explosionSound);
        }
    }
}

