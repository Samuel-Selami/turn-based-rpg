using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum BattleState { PlayerTurn, EnnemyTurn, Victory, Defeat }
public class BattleManager : MonoBehaviour
{
    [Header("List player / ennemy")]
    [SerializeField] List<Monster> playerTeam;
    [SerializeField] List<Monster> ennemyTeam;
    BattleManager manager;
    BattleState state;

    void Start()
    {
        foreach (Monster ally in ennemyTeam) ally.manager = this; // ennemyteam
        foreach (Monster ally in playerTeam) ally.manager = this; // playerteam
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop() // boucle de combat
    {
        state = BattleState.PlayerTurn;
        foreach (Monster ally in new List<Monster>(playerTeam))
        {
            if (ally.IsAlive())
                yield return StartCoroutine(ally.AttackSequence(PickTarget(ennemyTeam)));
        }
        state = BattleState.EnnemyTurn;
        foreach (Monster ally in new List<Monster>(ennemyTeam))
        {
            if (ally.IsAlive())
                yield return StartCoroutine(ally.AttackSequence(PickTarget(playerTeam)));
        }

        if (ennemyTeam.Count == 0)
        {
            state = BattleState.Victory;
            Debug.Log("Vous avez remportez le combat !");
        }
        else if (playerTeam.Count == 0)
        {
            state = BattleState.Defeat;
            Debug.Log("Vous avez été défait !");
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
