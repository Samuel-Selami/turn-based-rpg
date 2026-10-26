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

    [Header("Animations")]
    [SerializeField] float prepareTime = 0.35f;
    [SerializeField] float returnTime = 0.12f;
    Animator animator;

    public int currentHp;
    public int currentMana;
    Renderer monsterRender;
    public Monster chosenTarget;
    public AbilityData chosenAbility;
    public bool isGuarding = false;

    private void Awake()
    {
        if (data == null) return;

        currentHp = data.maxHp;
        currentMana = data.maxMana;
        monsterRender = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (data == null) return;
    }

    int ComputeDamage(int attackerAttack) // calcul de damage
    {
        int defense = data.defense; // revient à la normal
        if (isGuarding)
            defense = defense * 2;

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
            StartCoroutine(MonsterAnimation("Hit")); //anim takedamage
        Debug.Log(data.monsterName + " : " + currentHp + " / " + data.maxHp + "hp" + " et " + currentMana + " / " + data.maxMana + "mana");

    }

    void Heal(int amount) // calcul de heal reçu
    {
            currentHp = Mathf.Min((currentHp + amount), data.maxHp);
    }

    void ManaRecovery(int amount) // calcul de mana reçu
    {
        currentMana = Mathf.Min((currentMana + amount), data.maxMana);
    }

    public void ApplyAbility(AbilityData ability) // sur la cible
    {

        int amount = ability.power;

        if (ability.powerType == AbilityData.PowerType.Percent)
            amount = (data.maxHp * amount) / 100;

        if (ability.abilityEffect == AbilityData.AbilityEffect.Damage) // Damage
            TakeDamage(amount);
        if (ability.abilityEffect == AbilityData.AbilityEffect.Heal) // Heal
            Heal(amount);
        else // ManaRecovery
            ManaRecovery(amount);

    }

    public int AbilityCostValue(AbilityData ability)
    {
        int amount = ability.abilityCost;

        if (ability.abilityCostResource == AbilityData.AbilityCostResource.Mana)
        {
            if (ability.abilityCostType == AbilityData.AbilityCostType.Percent)
                amount = (data.maxMana * amount) / 100;
        }

        if (ability.abilityCostResource == AbilityData.AbilityCostResource.Hp)
        {
            if (ability.abilityCostType == AbilityData.AbilityCostType.Percent)
                amount = (data.maxHp * amount) / 100;
        }
        return amount;
    }

    public bool CanPayAbility(AbilityData ability)
    {
        int amount = AbilityCostValue(ability);

        if (ability.abilityCostResource == AbilityData.AbilityCostResource.Mana)
            return currentMana >= amount;
        else
            return currentHp >= amount;

    }
    public void PayCostAbility(AbilityData ability) // sur le lanceur
    {
    int amount = AbilityCostValue(ability);

        if (ability.abilityCostResource == AbilityData.AbilityCostResource.Mana)
                currentMana = Mathf.Max(0, (currentMana - amount));
        if (ability.abilityCostResource == AbilityData.AbilityCostResource.Hp)
                TakeDamage(amount);
    }


    public float HpRatio()
    {
        float ratio = (float)currentHp / data.maxHp;
        return ratio;
    }
    public float ManaRatio()
    {
        float ratio = (float)currentMana / data.maxMana;
        return ratio;
    }

    public bool IsAlive()
    {
        return currentHp > 0;
    }

    IEnumerator MonsterAnimation(string state)
    {
        animator.SetTrigger(state);
        yield return null;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(state))
            yield return null;
    }

    IEnumerator WaitUntil(float point)
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < point)
            yield return null;
    }

    public IEnumerator AttackSequence(Monster target) // séquence d'attaque
    {
        if (target == null || target.currentHp <= 0)
            yield break;

        // var
        Renderer targetrender = target.GetComponentInChildren<Renderer>();
        Color originalColor = targetrender.material.color;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 destination = target.transform.position - direction * data.stopDistance;
        Quaternion startRotation = transform.rotation;
        transform.LookAt(target.transform);

        //sequence :

        Debug.Log(data.monsterName + " s'élance.");
        Vector3 startPosition = transform.position;
        yield return new WaitForSeconds(prepareTime);

        animator.SetBool("Moving", true);
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, data.moveSpeed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("Moving", false);
        Debug.Log(data.monsterName + " attaque " + target.data.monsterName + ".");
        yield return StartCoroutine(MonsterAnimation("Attack")); //anim attack

        yield return StartCoroutine(WaitUntil(0.4f));
        target.TakeDamage(target.ComputeDamage(data.attack));
        targetrender.material.color = Color.red;
        yield return StartCoroutine(WaitUntil(1f));

        targetrender.material.color = originalColor;
        animator.SetBool("Moving", true);
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, data.moveSpeed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("Moving", false);
        transform.rotation = startRotation;
    }

    IEnumerator DeathSequence() // séquence de mort
    {
        yield return StartCoroutine(MonsterAnimation("Die")); //anim die
        Debug.Log(data.monsterName + " à été vaincu !");
        gameObject.SetActive(false);
        manager.RemoveFromBattle(this);
    }

}
