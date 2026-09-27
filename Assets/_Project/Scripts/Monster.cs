using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Others")]
    [SerializeField] public Monster target; //monstre cible

    [Header("Monster Attributes")]
    [SerializeField] MonsterData data; // scriptable objects

    [Header("Animation Time")]
    [SerializeField] float prepareTime = 0.35f;
    [SerializeField] float impactTime = 0.15f;
    [SerializeField] float returnTime = 0.12f;

    int currentHp;
    int currentQi;
    Renderer monsterRender;

    private void Awake()
    {
        if (data == null) return;

        currentHp = data.maxHp;
        currentQi = data.maxQi;
        monsterRender = GetComponent<Renderer>();
        monsterRender.material.color = data.monsterColor;
    }

    void Start()
    {
        if (data == null) return;
        StartCoroutine(AttackSequence(target));
    }


    int ComputeDamage(int attackerAttack) // calcul de damage
    {
        return Mathf.Max(1, (attackerAttack - data.defense)); // min 1 damage
    }

    void TakeDamage(int amount) // calcul de damage reçu
    {
        currentHp = Mathf.Max(0, (currentHp - amount));

        if (currentHp == 0)
        {
            StartCoroutine(DeathSequence());
        }
        else
            Debug.Log(data.monsterName + " : " + currentHp + " / " + data.maxHp + "hp" + " et " + currentQi + " / " + data.maxQi + "qi");

    }

    IEnumerator AttackSequence(Monster target) // séquence d'attaque
    {
        if (target == null || target.currentHp <= 0)
            yield break;

        Debug.Log(data.monsterName + " s'élance.");
        Vector3 startPosition = transform.position;
        Renderer targetrender = target.GetComponent<Renderer>();
        yield return new WaitForSeconds(prepareTime); // pause mais le reste du jeu continue.

        transform.position = startPosition + new Vector3(0f, 0f, 2f);
        Debug.Log(data.monsterName + " attaque " + target.data.monsterName + ".");
        yield return new WaitForSeconds(impactTime);

        Color originalColor = targetrender.material.color;
        target.TakeDamage(target.ComputeDamage(data.attack));
        targetrender.material.color = Color.red;
        yield return new WaitForSeconds(returnTime);

        targetrender.material.color = originalColor;
        transform.position = startPosition;
        Debug.Log(data.monsterName + " recule.");
    }

    IEnumerator DeathSequence() // séquence de mort
    {
        Debug.Log(data.monsterName + " à été vaincu !");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false); // fait disparaitre le gameobject
    }

}
