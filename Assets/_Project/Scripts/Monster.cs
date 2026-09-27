using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Others")]
    [SerializeField] public Monster target; //monstre cible

    [Header("Monster Attributes")]
    [SerializeField] string monsterName = "Unknow";
    [SerializeField] int maxHp = 250;
    [SerializeField] int attack = 25;
    [SerializeField] int defense = 35;
    [SerializeField] int qi = 10;

    [Header("Animation Time")]
    [SerializeField] float prepareTime = 0.35f;
    [SerializeField] float impactTime = 0.15f;
    [SerializeField] float returnTime = 0.12f;

    int currentHp;

    void Start()
    {
        currentHp = maxHp;
        StartCoroutine(AttackSequence(target));
    }


    int ComputeDamage(int attackerAttack) // calcul de damage
    {
        return Mathf.Max(1, (attackerAttack - defense)); // min 1 damage
    }

    void TakeDamage(int amount) // calcul de damage reçu
    {
        currentHp = Mathf.Max(0, (currentHp - amount));

        if (currentHp == 0)
        {
            StartCoroutine(DeathSequence());
        }
        else
            Debug.Log(monsterName + " : " + currentHp + " / " + maxHp);

    }

    IEnumerator AttackSequence(Monster target) // séquence d'attaque
    {
        if (target == null || target.currentHp <= 0)
            yield break;

        Debug.Log(monsterName + " s'élance.");
        Vector3 startPosition = transform.position;
        Renderer targetrender = target.GetComponent<Renderer>();
        yield return new WaitForSeconds(prepareTime); // pause mais le reste du jeu continue.

        transform.position = startPosition + new Vector3(0f, 0f, 2f);
        Debug.Log(monsterName + " attaque " + target.monsterName + ".");
        yield return new WaitForSeconds(impactTime);

        Color originalColor = targetrender.material.color;
        target.TakeDamage(target.ComputeDamage(attack));
        targetrender.material.color = Color.red;
        yield return new WaitForSeconds(returnTime);

        targetrender.material.color = originalColor;
        transform.position = startPosition;
        Debug.Log(monsterName + " recule.");
    }

    IEnumerator DeathSequence() // séquence de mort
    {
        Debug.Log(monsterName + " à été vaincu !");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false); // fait disparaitre le gameobject
    }

}
