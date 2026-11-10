using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    public static ScreenFader instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public IEnumerator Fade(float from, float to, float fadeDuration)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;


            Color c = fadeScreen.color;
            c.a = Mathf.Lerp(from, to, t);
            fadeScreen.color = c;

            yield return null;
        }
    }
    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }
}
