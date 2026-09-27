using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public Color monsterColor;
    public string monsterName;
    public int maxHp;
    public int attack;
    public int defense;
    public int maxQi;
    public int speed;
}
