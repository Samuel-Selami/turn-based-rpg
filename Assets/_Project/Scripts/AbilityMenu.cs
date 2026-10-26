using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour
{
    [SerializeField] GameObject abilityRowPrefab;
    [SerializeField] Transform AbilityList;
    [SerializeField] TMP_Text currentmanaText;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_Text abilityDescription;
    [SerializeField] TMP_Text abilityInfos;


    List<AbilityRow> rows = new List<AbilityRow>();
    List<AbilityData> abilities;
    int index;
    public bool chosen;
    public bool cancelled;


    void Highlight(int i)
    {
        for (int j = 0; j < rows.Count; j++)
            rows[j].SetHighlighted(false);
        rows[i].SetHighlighted(true);
        if (rows.Count > 1)
            scrollRect.verticalNormalizedPosition = 1f - (float)i / (rows.Count - 1);

        abilityDescription.text = abilities[i].FormattedDescription();
        abilityInfos.text = abilities[i].abilityType + " • " + abilities[i].targetMode + " • " + abilities[i].accuracy + "%";
    }
    public void Open(Monster monster)
    {
        foreach (AbilityRow row in rows)
            Destroy(row.gameObject);
        rows.Clear();

        abilities = monster.data.abilities;
        currentmanaText.text = monster.currentMana + " / " + monster.data.maxMana;

        foreach (AbilityData a in abilities)
        {
            GameObject go = Instantiate(abilityRowPrefab, AbilityList);
            AbilityRow row = go.GetComponent<AbilityRow>();
            row.Fill(a);
            rows.Add(row);
        }

        index = 0;
        chosen = false;
        cancelled = false;
        Highlight(index);
    }

    public AbilityData Chosen()
    {
        return abilities[index];
    }

    private void Update()
    {
        if (rows.Count == 0) return;

        int v = BattleInput.Vertical();
        if (v != 0)
        {
            index = (index + v + rows.Count) % rows.Count;
            Highlight(index);
        }

        if (BattleInput.Confirm()) chosen = true;
        if (BattleInput.Cancel()) cancelled = true;

    }
}
