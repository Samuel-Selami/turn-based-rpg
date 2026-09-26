using JetBrains.Annotations;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [SerializeField] string monsterName = "Unknow";
    [SerializeField] int maxHp = 250;
    [SerializeField] int attack = 25;
    [SerializeField] int defense = 35;

    int currentHp;

    void Start()
    {
        currentHp = maxHp;
        int damage = ComputeDamage(attack);
        TakeDamage(damage);
    }


    int ComputeDamage(int attackerAttack)
    {
        return Mathf.Max(1, (attackerAttack - defense));
    }

    void TakeDamage(int amount)
    {
        currentHp = Mathf.Max(0, (currentHp - amount));

        if (currentHp == 0)
        {
            Debug.Log("Le " + monsterName + " a été vaincu !");
        }
        else
            Debug.Log(monsterName + " : " + currentHp + " / " + maxHp);

    }
}
