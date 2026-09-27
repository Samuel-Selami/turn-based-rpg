using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Monster Attributes")]
    [SerializeField] string monsterName = "Unknow";
    [SerializeField] int maxHp = 250;
    [SerializeField] int attack = 25;
    [SerializeField] int defense = 35;

    [Header("Animation Time")]
    [SerializeField] float prepareTime = 0.35f;
    [SerializeField] float impactTime = 0.15f;
    [SerializeField] float returnTime = 0.12f;

    int currentHp;

    void Start()
    {
        currentHp = maxHp;
        StartCoroutine(AttackSequence(ComputeDamage(attack)));
    }


    int ComputeDamage(int attackerAttack)
    {
        return Mathf.Max(1, (attackerAttack - defense)); // min 1 damage
    }

    void TakeDamage(int amount)
    {
        currentHp = Mathf.Max(0, (currentHp - amount));

        if (currentHp == 0)
        {
            StartCoroutine(DeathSequence());
        }
        else
            Debug.Log(monsterName + " : " + currentHp + " / " + maxHp);

    }

    IEnumerator AttackSequence(int damage)
    {
        Debug.Log(monsterName + " s'élance.");
        Vector3 startPosition = transform.position;
        Renderer rend = GetComponent<Renderer>();

        yield return new WaitForSeconds(prepareTime); // pause mais le reste du jeu continue.
        transform.position = startPosition + new Vector3(0f, 0f, 2f);

        yield return new WaitForSeconds(impactTime);
        Color originalColor = rend.material.color;

        TakeDamage(damage);
        rend.material.color = Color.red;
        yield return new WaitForSeconds(returnTime);

        rend.material.color = originalColor;
        transform.position = startPosition;
        Debug.Log(monsterName + " recule.");
    }

    IEnumerator DeathSequence()
    {
        Debug.Log(monsterName + " à été vaincu !");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false); // fait disparaitre le gameobject
    }

}
