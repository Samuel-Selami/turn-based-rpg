using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("List player / ennemy")]
    [SerializeField] List<Monster> playerTeam;
    [SerializeField] List<Monster> ennemyTeam;
    BattleManager manager;

    void Start()
    {

        foreach (Monster ally in ennemyTeam) // ennemyteam
        {
            ally.manager = this;
            Debug.Log(ally.data.monsterName + " : " + ally.data.maxHp + "hp");
            ally.Attack(PickTarget(playerTeam));
        }

        foreach (Monster ally in playerTeam) // playerteam
        {
            ally.manager = this;
            Debug.Log(ally.data.monsterName + " : " + ally.data.maxHp + "hp");
            ally.Attack(PickTarget(ennemyTeam));
        }
    }

    void Update()
    {

    }

    Monster PickTarget(List<Monster> team) // chosisir la team (player / ennemy)
    {
        if (team.Count == 0)
            return null;
        int index = Random.Range(0, team.Count);
        return team[index];
    }

    public void RemoveFromBattle(Monster m)
    {
        playerTeam.Remove(m);
        ennemyTeam.Remove(m);
    }
}
