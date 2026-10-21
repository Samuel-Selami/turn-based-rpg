using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMenu : MonoBehaviour
{
    public bool fightChosen = false;
    [SerializeField] List<RectTransform> actionButtons;
    [SerializeField] RectTransform selectionArrow;
    [SerializeField] Vector2 arrowOffset;
    int index = 0;


    void Highlight (int i)
    {
        for (int j = 0; j < actionButtons.Count; j++)
        {
            actionButtons[j].transform.localScale = Vector3.one;
        }
        actionButtons[i].transform.localScale = Vector3.one * 1.1f;
    }
    private void Update()
    {
        int direction = BattleInput.Vertical();

            if (direction != 0)
            {
                index = (index + direction + actionButtons.Count) % actionButtons.Count;
                selectionArrow.SetParent(actionButtons[index], false);
                selectionArrow.anchoredPosition = arrowOffset;
            Highlight(index);
            }

        if (BattleInput.Confirm())
        {
            if (index == 0)
                fightChosen = true;
            else
                Debug.Log("Pas encore disponible");
        }

    }
    public void ChooseFight()
    {
        fightChosen = true;
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
