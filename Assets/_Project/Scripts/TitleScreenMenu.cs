using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;


[System.Serializable]
public class TitleScreenSelection
{
    public RectTransform button;
    public GameObject highlightBorder;
    public GameObject highlightStarLeft;
    public GameObject highlightStarRight;


    public void SetHighlighted(bool on)
    {
        highlightBorder.SetActive(on);
        highlightStarLeft.SetActive(on);
        highlightStarRight.SetActive(on);

        float scale = 1f;
        if (on)
            scale = 1.2f;
        button.localScale = Vector3.one * scale; // scale up
    }
}

public class TitleScreenMenu : MonoBehaviour
{
    [SerializeField] List<TitleScreenSelection> entries;
    [SerializeField] RectTransform selectionArrow;
    int index;
    Vector3 arrowTarget;
    [SerializeField] float arrowSpeed = 1f;

    private void Start()
    {
        Highlight(0);
    }

    void Highlight(int i)
    {
        for (int j = 0; j < entries.Count; j++)
            entries[j].SetHighlighted(false);
        entries[i].SetHighlighted(true);

        arrowTarget = selectionArrow.position;
        arrowTarget.y = entries[i].button.position.y;
    }
    void Update()
    {
        if (entries.Count == 0) return;

        int v = BattleInput.Vertical();
        if (v != 0)
        {
            index = (index + v + entries.Count) % entries.Count;
            Highlight(index);
        }
        selectionArrow.position = Vector3.Lerp(selectionArrow.position, arrowTarget, arrowSpeed * Time.deltaTime);
    }
}
