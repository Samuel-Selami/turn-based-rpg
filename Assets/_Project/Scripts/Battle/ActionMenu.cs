using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType { Fight, Abilities, Guard, Objects, Pact };

[System.Serializable]
public class MenuEntry
{
    public RectTransform button;
    public GameObject highlight;
    public int up, down, left, right;
}

public class ActionMenu : MonoBehaviour
{
    public bool actionChosen = false;
    [SerializeField] List<MenuEntry> entries;
    [SerializeField] RectTransform selectionArrow;
    int index = 0;

    public ActionType chosenAction;

    void Highlight(int i)
    {
        for (int j = 0; j < entries.Count; j++)
        {
            entries[j].button.localScale = Vector3.one;
            entries[j].highlight.SetActive(false);
        }

        entries[i].button.localScale = Vector3.one * 1.05f; //scale up
        entries[i].highlight.SetActive(true);
    }

    private void Update()
    {
        int v = BattleInput.Vertical();
        int h = BattleInput.Horizontal();

        if (v != 0 || h != 0)
        {
            if (v > 0) index = entries[index].down;
            else if (v < 0) index = entries[index].up;
            else if (h > 0) index = entries[index].right;
            else index = entries[index].left;

            Highlight(index);
        }

        if (BattleInput.Confirm())
        {
            chosenAction = (ActionType)index;
            actionChosen = true;
        }

    }
    public void ChooseFight()
    {
        actionChosen = true;
    }

    public void ChooseAbilities()
    {
        Debug.Log("Fight");
    }

    public void ChooseGuard()
    {
        Debug.Log("Fight");
    }

    public void ChooseObjects()
    {
        Debug.Log("Fight");
    }

    public void ChoosePact()
    {
        Debug.Log("Fight");
    }
}
