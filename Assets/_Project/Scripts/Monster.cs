using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class Monster : MonoBehaviour
{
    [Header("Others")]
    public BattleManager manager;

    [Header("Monster Attributes")]
    [SerializeField] public MonsterData data; // scriptable objects

    [Header("Animation Time")]
    [SerializeField] float prepareTime = 0.35f;
    [SerializeField] float impactTime = 0.15f;
    [SerializeField] float returnTime = 0.12f;
    Animator animator;

    public int currentHp;
    int currentQi;
    Renderer monsterRender;

    private void Awake()
    {
        if (data == null) return;

        currentHp = data.maxHp;
        currentQi = data.maxQi;
        monsterRender = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (data == null) return;
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
            animator.SetTrigger("Hit"); //anim de takedamage
        Debug.Log(data.monsterName + " : " + currentHp + " / " + data.maxHp + "hp" + " et " + currentQi + " / " + data.maxQi + "qi");

    }

    public bool IsAlive()
    {
        return currentHp >= 0;
    }

    public IEnumerator AttackSequence(Monster target) // séquence d'attaque
    {
        if (target == null || target.currentHp <= 0)
            yield break;
        // var
        Renderer targetrender = target.GetComponentInChildren<Renderer>();
        Color originalColor = targetrender.material.color;

        //sequence :
        Debug.Log(data.monsterName + " s'élance.");
        Vector3 startPosition = transform.position;
        yield return new WaitForSeconds(prepareTime); // pause mais le reste du jeu continue.

        transform.position = startPosition + new Vector3(0f, 0f, 2f);
        // anim moove on et off
        Debug.Log(data.monsterName + " attaque " + target.data.monsterName + ".");
        yield return new WaitForSeconds(impactTime);

        animator.SetTrigger("Attack"); //anim d'attaque
        target.TakeDamage(target.ComputeDamage(data.attack));
        targetrender.material.color = Color.red;
        yield return new WaitForSeconds(returnTime);

        targetrender.material.color = originalColor;
        //anim moove on et off
        transform.position = startPosition;
        //Debug.Log(data.monsterName + " recule.");
    }

    IEnumerator DeathSequence() // séquence de mort
    {
        animator.SetTrigger("Die");
        Debug.Log(data.monsterName + " à été vaincu !");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false); // fait disparaitre le gameobject
        manager.RemoveFromBattle(this);
    }

}
