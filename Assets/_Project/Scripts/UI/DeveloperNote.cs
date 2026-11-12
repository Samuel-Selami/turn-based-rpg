using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperNote : MonoBehaviour
{

    [SerializeField] CanvasGroup textNote;
    [SerializeField] CanvasGroup background;
    [SerializeField] float readDuration = 3f;
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeOutDuration = 1f;

    void Start()
    {

    }

    void Update()
    {

    }

    public IEnumerator Show()
    {
        yield return FadeGroup(background, 1f, 1f, fadeInDuration);
        yield return FadeGroup(textNote, 0f, 1f, fadeInDuration);
        yield return new WaitForSeconds(readDuration);
        yield return FadeGroup(textNote, 1f, 0f, fadeOutDuration);
        background.alpha = 0f;
        //yield return FadeGroup(background, 1f, 0f, fadeOutDuration);
    }

    IEnumerator FadeGroup(CanvasGroup group, float from, float to, float fadeDuration)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            
            group.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }
    }
}
