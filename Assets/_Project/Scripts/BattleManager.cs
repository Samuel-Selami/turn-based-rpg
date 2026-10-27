using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    [SerializeField] GameObject selectionSpellAllyVfx;
    [SerializeField] GameObject selectionEnnemyVfx;
    [SerializeField] GameObject spellCastVfx;
    [SerializeField] public GameObject guardVfx;
    [SerializeField] GameObject enemyTagPrefab;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] AbilityMenu abilityMenu;
    [SerializeField] float popupHeight = 3f;
    [SerializeField, Range(0f, 1f)] float impactPoint = 0.5f;
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

            foreach (Monster ally in playerTeam)
                ally.StopGuard();

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
                int j = i - 1;
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

                if (ally.IsAlive() && ally.chosenGuard)
                {
                    ally.StartGuard();
                    ally.chosenGuard = false;
                    continue;

                }

                if (ally.IsAlive() && ally.chosenTarget != null && !ally.chosenTarget.IsAlive())
                {
                    if (ally.chosenAbility != null && (ally.chosenAbility.targetSide == AbilityData.TargetSide.Allies || ally.chosenAbility.targetSide == AbilityData.TargetSide.Self))
                        ally.chosenTarget = null;
                    else
                        ally.chosenTarget = PickTarget(ennemyTeam);
                }

                if (ally.IsAlive() && ally.chosenTarget != null)
                {
                    if (ally.chosenAbility != null)
                    {
                        ally.PayCostAbility(ally.chosenAbility);
                        yield return StartCoroutine(CastSequence(ally));

                        List<Monster> targets = new List<Monster>();

                        if (ally.chosenAbility.targetMode == AbilityData.TargetMode.All)
                        {
                            List<Monster> team = ennemyTeam;
                            if (ally.chosenAbility.targetSide == AbilityData.TargetSide.Allies)
                                team = playerTeam;


                            targets = new List<Monster>(team);
                        }
                        else
                        {
                            targets.Add(ally.chosenTarget);
                        }

                        yield return StartCoroutine(AbilityVfxSequence(ally.chosenAbility, targets));

                        ally.chosenAbility = null;
                    }
                    else
                    {
                        yield return StartCoroutine(ally.AttackSequence(ally.chosenTarget));
                    }

                    ally.chosenTarget = null;
                }
            }

            // ENNEMY
            state = BattleState.EnnemyTurn;
            foreach (Monster ally in new List<Monster>(ennemyTeam))
            {
                if (!ally.IsAlive())
                    continue;

                AbilityData ability = PickEnemyAbility(ally);

                if (ability == null)
                {
                    yield return StartCoroutine(ally.AttackSequence(PickTarget(playerTeam)));
                    continue;
                }

                ally.PayCostAbility(ability);
                yield return StartCoroutine(CastSequence(ally));

                List<Monster> team = playerTeam;
                List<Monster> targets = new List<Monster>();

                if (ability.targetSide == AbilityData.TargetSide.Allies)
                    team = ennemyTeam;

                if (ability.targetSide == AbilityData.TargetSide.Self)
                    targets.Add(ally);
                else if (ability.targetMode == AbilityData.TargetMode.All)
                    targets = new List<Monster>(team);
                else
                {
                    Monster target = PickTarget(team);
                    if (target != null)
                        targets.Add(target);
                }

                yield return StartCoroutine(AbilityVfxSequence(ability, targets));
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
        ally.chosenGuard = false;
        ally.chosenAbility = null;

        GameObject allyVfx = Instantiate(selectionAllyVfx, ally.transform); // VFX ally selection spawn
                                                                            //Panel
        playerPanels[panelIndex].transform.localScale = Vector3.one * 1.2f; // pannel scale up
        yield return
        StartCoroutine(playerPanels[panelIndex].SlideMonsterPanelSelection(true)); //anim slide selection

        int index = 0;
        GameObject ennemyVfx = null;


        bool ordered = false;

        while (!ordered) // --------------------------
        {
            bool confirmed = false;
            bool cancelled = false;
            ally.chosenAbility = null;

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
                ally.chosenGuard = true;
                ordered = true;
                continue;
            }
            if (actionMenu.chosenAction == ActionType.Abilities)
            {
                if (ally.data.abilities.Count == 0)
                {
                    yield return null;
                    continue;
                }

                abilityMenu.gameObject.SetActive(true);
                yield return null;
                abilityMenu.Open(ally);

                while (!abilityMenu.chosen && !abilityMenu.cancelled)
                    yield return null;

                abilityMenu.gameObject.SetActive(false);

                if (abilityMenu.cancelled)
                {
                    yield return null;
                    continue;
                }

                if (!ally.CanPayAbility(abilityMenu.Chosen()))
                {
                    yield return null;
                    continue;
                }

                ally.chosenAbility = abilityMenu.Chosen();
            }

            else if (actionMenu.chosenAction != ActionType.Fight)
            {
                yield return null;
                continue;
            }

            List<Monster> targetTeam = ennemyTeam;
            GameObject selectionMonsterPrefab = selectionEnnemyVfx;

            if (ally.chosenAbility != null)
            {
                if (ally.chosenAbility.targetSide == AbilityData.TargetSide.Allies)
                {
                    targetTeam = playerTeam;
                    selectionMonsterPrefab = selectionSpellAllyVfx;
                }

                if (ally.chosenAbility.targetSide == AbilityData.TargetSide.Self)
                {
                    ally.chosenTarget = ally;
                    ordered = true;
                    continue;
                }
            }

            if (index >= targetTeam.Count)
                index = 0;

            if (ally.chosenAbility != null && ally.chosenAbility.targetMode == AbilityData.TargetMode.All)
            {
                yield return null;

                List<GameObject> cursors = new List<GameObject>();
                foreach (Monster m in targetTeam)
                    cursors.Add(Instantiate(selectionMonsterPrefab, m.transform));

                bool allConfirmed = false;
                bool allCancelled = false;

                while (!allConfirmed && !allCancelled)
                {
                    if (BattleInput.Confirm()) allConfirmed = true;
                    if (BattleInput.Cancel()) allCancelled = true;
                    yield return null;
                }

                foreach (GameObject c in cursors)
                    Destroy(c);

                if (allCancelled)
                    continue;

                ally.chosenTarget = ally;
                ordered = true;
                continue;
            }

            yield return null;
            ennemyVfx = Instantiate(selectionMonsterPrefab, targetTeam[index].transform);// VFX enenmy selection first spawn

            while (!confirmed && !cancelled)
            {
                if (BattleInput.Confirm())
                    confirmed = true;
                if (BattleInput.Cancel())
                    cancelled = true;

                int direction = BattleInput.Horizontal();
                if (direction != 0) //select moove ennemy
                {
                    index = (index + direction + targetTeam.Count) % targetTeam.Count;
                    Destroy(ennemyVfx);
                    ennemyVfx = Instantiate(selectionMonsterPrefab, targetTeam[index].transform);// VFX enenmy selection spawn
                }
                yield return null;
            }

            if (confirmed)
            {
                ally.chosenTarget = targetTeam[index];
                ordered = true;
            }

            Destroy(ennemyVfx);
        }
        // Panel
        playerPanels[panelIndex].transform.localScale = Vector3.one; //pannel normal scale
        yield return
        StartCoroutine(playerPanels[panelIndex].SlideMonsterPanelSelection(false));

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

    AbilityData PickEnemyAbility(Monster enemy)
    {
        List<AbilityData> usable = new List<AbilityData>();

        foreach (AbilityData a in enemy.data.abilities)
        {
            if (enemy.CanPayAbility(a))
                usable.Add(a);
        }

        if (usable.Count == 0)
            return null;

        if (Random.Range(0, 100) < 50) // 50 %  de chance de faire une attaque normal (fight)
            return null;

        return usable[Random.Range(0, usable.Count)];
    }

    public IEnumerator CastSequence(Monster caster)
    {
        GameObject spellCast = Instantiate(spellCastVfx, caster.transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(spellCast);
    }

    void ApplyToAll(AbilityData ability, List<Monster> targets)
    {
        foreach (Monster m in targets)
            m.ApplyAbility(ability);
    }

    public IEnumerator AbilityVfxSequence(AbilityData ability, List<Monster> targets)
    {
        List<GameObject> vfxList = new List<GameObject>();

        if (targets.Count == 0 || ability.abilityVfx == null)
        {
            ApplyToAll(ability, targets);
            yield break;
        }

        foreach (Monster m in targets)
        {
            GameObject vfx = Instantiate(ability.abilityVfx, m.transform);
            vfxList.Add(vfx);
        }

        ParticleSystem ps = vfxList[0].GetComponentInChildren<ParticleSystem>();
        float duration = ps.main.duration;

        yield return new WaitForSeconds(duration * impactPoint);
        ApplyToAll(ability, targets);
        yield return new WaitForSeconds(duration * (1f - impactPoint));


        foreach (GameObject g in vfxList)
            Destroy(g);
    }

    public void ShowDamagePopup(Monster target, int amount, bool isHeal)
    {
        GameObject damagepopup = Instantiate(damagePopupPrefab, target.transform.position + Vector3.up * popupHeight, Quaternion.identity);
        damagepopup.GetComponent<DamagePopup>().Show(amount, isHeal);
    }
    public void RemoveFromBattle(Monster m)
    {
        playerTeam.Remove(m);
        ennemyTeam.Remove(m);
    }
}
