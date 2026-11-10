using TMPro;
using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour
{

    [SerializeField] TMP_Text damageText;
    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;

    [Header("Animation")]
    [SerializeField] float lifetime = 0.8f;
    [SerializeField] float riseHeight = 1.5f;
    [SerializeField] AnimationCurve scaleCurve;


    public void Show(int amount, bool isHeal)
    {
        // info text
        damageText.text = amount.ToString();
        // couleur
        if (isHeal)
            damageText.color = healColor;
        else
            damageText.color = damageColor;

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lifetime; // t 0 à 1

            transform.position =  startPosition + Vector3.up * (riseHeight * t);

            Color c = damageText.color;
            c.a = 1f - t; // opacity over time
            damageText.color = c;

            transform.localScale = Vector3.one * scaleCurve.Evaluate(t);

            yield return null;
        }

        Destroy(gameObject);
    }

}
