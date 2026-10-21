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
    [SerializeField] List<MonsterPanel> playerPanels;
    [SerializeField] ActionMenu actionMenu;
    List<Monster> chosenActor = new List<Monster>();

    private void Awake()
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            playerPanels[i].monster = playerTeam[i];
        }
    }

    void Start()
    {
        foreach (Monster ally in ennemyTeam) ally.manager = this; // ennemyteam
        foreach (Monster ally in playerTeam) ally.manager = this; // playerteam
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop() // boucle de combat
    {
        while (playerTeam.Count > 0 && ennemyTeam.Count > 0)
        {
            // PLAYER
            state = BattleState.PlayerTurn;

            //demande
            chosenActor.Clear();
            for (int i = 0; i < playerPanels.Count; i++)
            {
                {
                    Monster ally = playerPanels[i].monster;
                    if (ally.IsAlive())
                    {
                        playerPanels[i].transform.localScale = Vector3.one * 1.2f; //pannel scale up
                        actionMenu.fightChosen = false;
                        actionMenu.gameObject.SetActive(true);

                        while (!actionMenu.fightChosen)
                            yield return null;

                        actionMenu.gameObject.SetActive(false);
                        playerPanels[i].transform.localScale = Vector3.one; //pannel normal scale
                        chosenActor.Add(ally);
                    }
                }
            }

                //action
                foreach (Monster ally in chosenActor)
                {
                    if (ally.IsAlive())
                    {
                        yield return StartCoroutine(ally.AttackSequence(PickTarget(ennemyTeam)));
                    }
                }

                // ENNEMY
                state = BattleState.EnnemyTurn;
                foreach (Monster ally in new List<Monster>(ennemyTeam))
                {
                    if (ally.IsAlive())
                        yield return StartCoroutine(ally.AttackSequence(PickTarget(playerTeam)));
                }
            }
            // VICTORY
            if (ennemyTeam.Count == 0)
            {
                state = BattleState.Victory;
                Debug.Log("Vous avez remportez le combat !");
            }
            //DEFEAT
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
