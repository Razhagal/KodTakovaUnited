using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingFX : MonoBehaviour
{
    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {
        float currentAlpha = 0f;
        Color originalColor = renderer.color;

        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);

        float speed = 3f;
        while (currentAlpha < 1f)
        {
            currentAlpha += Time.deltaTime * speed;

            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        currentAlpha = 1f;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);

        speed = 2.5f;
        while (currentAlpha > 0f)
        {
            currentAlpha -= Time.deltaTime * speed;

            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        currentAlpha = 0f;
        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);

        Destroy(gameObject);
    }
}
