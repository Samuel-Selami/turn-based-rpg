using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MonsterPanel : MonoBehaviour
{
    [Header("Monster Infos")]

    [SerializeField]public Monster monster;
    [SerializeField] TMP_Text monsterName;
    [SerializeField] UnityEngine.UI.Image healthBar;
    [SerializeField] TMP_Text health;
    [SerializeField] UnityEngine.UI.Image manaBar;
    [SerializeField] TMP_Text mana;



    void Update()
    {
        // Monster
        monsterName.text = monster.data.monsterName + "";
        // Health
        healthBar.fillAmount = monster.HpRatio();
        health.text = monster.currentHp + "";
        // Mana
        manaBar.fillAmount = monster.ManaRatio();
        mana.text = monster.currentMana + "";
    }
}
