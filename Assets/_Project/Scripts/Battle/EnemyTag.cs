using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTag : MonoBehaviour
{
    [Header("Automatic Monster Index")]
    public Monster monster;
    [Header("Monster Name")]
    [SerializeField] TMP_Text monsterEnnemyName;
    [Header("Health Bar Green")]
    [SerializeField] Image healthBar;
    [SerializeField] float healthSpeed;
    [Header("Damage Health Bar Red")]
    [SerializeField] Image damageBar;
    [SerializeField] float damageSpeed;

    void Start()
    {
        monsterEnnemyName.text = monster.data.monsterName;
    }

    void Update()
    {
        // Life Drain Animation UI
        float ratio = monster.HpRatio();
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, ratio, healthSpeed * Time.deltaTime);
        damageBar.fillAmount = Mathf.MoveTowards(damageBar.fillAmount, ratio, damageSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
