using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("Monster Attributes")]
    public Color monsterColor;
    public Sprite monsterProfil;
    public string monsterName;
    public int maxHp;
    public int attack;
    public int defense;
    public int maxMana;
    public int speed;

    [Header("Animations")]
    public float stopDistance;
    public float moveSpeed;
}
