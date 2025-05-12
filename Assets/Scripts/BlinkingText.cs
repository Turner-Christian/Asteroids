using UnityEngine;
using System.Collections;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager object
    public TMP_Text text;
    private Color originalColor;

    void Start()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        originalColor = text.color;
        StartBlinking();
    }

    public void StartBlinking()
    {
        // Check the high score flag and start the blinking process accordingly
        if (gameManager.isHighScore)
        {
            StartCoroutine(Blink());
        }
        else
        {
            SetAlpha(0f); // Make text invisible if not a high score
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            SetAlpha(0f); // Make text invisible
            yield return new WaitForSeconds(0.5f);
            SetAlpha(1f); // Make text visible
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SetAlpha(float alpha)
    {
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
}
