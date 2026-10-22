using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MonsterPanel : MonoBehaviour
{
    [Header("Monster Infos")]

    [SerializeField]public Monster monster;
    [SerializeField] TMP_Text monsterName;
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text mana;
    [SerializeField] UnityEngine.UI.Image monsterProfil;
    [Header("Health Bar Green")]
    [SerializeField] UnityEngine.UI.Image healthBar;
    [SerializeField] float healthSpeed;
    [Header("Damage Health Bar Red")]
    [SerializeField] UnityEngine.UI.Image damageBar;
    [SerializeField] float damageSpeed;
    [Header("Mana Bar Blue")]
    [SerializeField] UnityEngine.UI.Image manaBar;
    [SerializeField] float manaSpeed;
    [Header("Damage Mana Bar")]
    [SerializeField] UnityEngine.UI.Image manaDamageBar;
    [SerializeField] float manaDamageSpeed;


    void Start()
    {
        monsterProfil.sprite = monster.data.monsterProfil;
    }

    void Update()
    {
        // Monster
        monsterName.text = monster.data.monsterName.ToString();
        // Health
        //animUI
        float ratio = monster.HpRatio();
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, ratio, healthSpeed * Time.deltaTime);
        damageBar.fillAmount = Mathf.MoveTowards(damageBar.fillAmount, ratio, damageSpeed * Time.deltaTime);

        health.text = monster.currentHp.ToString();
        // Mana
        //anim UI
        float manaratio = monster.ManaRatio();
        manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, manaratio, manaSpeed * Time.deltaTime);
        manaDamageBar.fillAmount = Mathf.MoveTowards(manaDamageBar.fillAmount, manaratio, manaDamageSpeed * Time.deltaTime);

        mana.text = monster.currentMana.ToString();
    }
}
