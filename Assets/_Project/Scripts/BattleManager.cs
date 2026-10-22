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

    [SerializeField] GameObject selectionAllyVfx;
    [SerializeField] GameObject selectionEnnemyVfx;
    [SerializeField] GameObject enemyTagPrefab;
    bool commandCancelled;

    private void Awake()
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            playerPanels[i].monster = playerTeam[i];
        }

        foreach (Monster enemy in ennemyTeam)
        {
            GameObject tag = Instantiate(enemyTagPrefab, enemy.transform);
            tag.GetComponent<EnemyTag>().monster = enemy;
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
            int i = 0;
            while (i < playerPanels.Count)
            {
                Monster ally = playerPanels[i].monster;

                if (!ally.IsAlive()) // si il est mort +1
                {
                    i++;
                    continue;
                }

                yield return StartCoroutine(CommandAlly(i));

                if (!commandCancelled) // si il a donné son ordre +1
                {
                    i++;
                    continue;
                }

                // si il a annulé -1
                int j = 1 - 1;
                while (j >= 0 && !playerPanels[j].monster.IsAlive())
                    j--;

                if (j >= 0) // si le 1er annule, on ne bouge pas 
                {
                    playerPanels[j].monster.chosenTarget = null;
                    i = j;
                }
            }

                //action
                for (int a = 0; a < playerPanels.Count; a++)
                {
                    Monster ally = playerPanels[a].monster;
                    if (ally.IsAlive() && ally.chosenTarget != null)
                        {
                        yield return StartCoroutine(ally.AttackSequence(ally.chosenTarget));
                        ally.chosenTarget = null;
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

    IEnumerator CommandAlly(int panelIndex) // Commande d'un tour d'un monstre ally
    {
        commandCancelled = false;
        Monster ally = playerPanels[panelIndex].monster;
        ally.isGuarding = false;

        GameObject allyVfx = Instantiate(selectionAllyVfx, ally.transform); // VFX ally selection spawn
        playerPanels[panelIndex].transform.localScale = Vector3.one * 1.2f; // pannel scale up
        int index = 0;
        GameObject ennemyVfx = null;


        bool ordered = false;

        while (!ordered) // --------------------------
        {
            bool confirmed = false;
            bool cancelled = false;

            actionMenu.actionChosen = false;
            actionMenu.gameObject.SetActive(true);

            bool menuCancelled = false;
            while (!actionMenu.actionChosen && !menuCancelled)
            {
                if (BattleInput.Cancel())
                    menuCancelled = true;
                yield return null;
            }

            actionMenu.gameObject.SetActive(false);
            if (menuCancelled)
            {
                commandCancelled = true;
                ordered = true;
                continue;
            }
            if (actionMenu.chosenAction == ActionType.Guard) //Guard
            {
                ally.isGuarding = true;
                ordered = true;
                continue;
            }
            if (actionMenu.chosenAction != ActionType.Fight)
                continue;

            yield return null;

            ennemyVfx = Instantiate(selectionEnnemyVfx, ennemyTeam[index].transform);// VFX enenmy selection first spawn

            while (!confirmed && !cancelled)
            {
                if (BattleInput.Confirm())
                    confirmed = true;
                if (BattleInput.Cancel())
                    cancelled = true;

                int direction = BattleInput.Horizontal();
                if (direction != 0) //select moove ennemy
                {
                    index = (index + direction + ennemyTeam.Count) % ennemyTeam.Count;
                    Destroy(ennemyVfx);
                    ennemyVfx = Instantiate(selectionEnnemyVfx, ennemyTeam[index].transform);// VFX enenmy selection spawn
                }
                yield return null;
            }

            if (confirmed)
            {
                ally.chosenTarget = ennemyTeam[index];
                ordered = true;
            }

            Destroy(ennemyVfx);
        }

        playerPanels[panelIndex].transform.localScale = Vector3.one; //pannel normal scale
        Destroy(allyVfx);
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
